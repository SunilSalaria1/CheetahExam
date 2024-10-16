using CheetahExam.WebUI.Shared.Common.Models;
using CheetahExam.WebUI.Shared.Common.Models.Exams;
using CheetahExam.WebUI.Shared.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using static CheetahExam.WebUI.Shared.Constants.Constant;
using static CheetahExam.WebUI.Shared.Enums.Enum;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions;

public partial class Index
{
    #region Fields

    [Parameter] public string? ExamId { get; set; }

    [Inject] IFilesClient FilesClient { get; set; } = null!;

    [Inject] IQuestionsClient QuestionsClient { get; set; } = null!;

    [Inject] IOptionsClient OptionsClient { get; set; } = null!;

    [Inject] IDialogService Dialog { get; set; } = null!;

    [Comment("This property is being used to prevent user from accessing Left panel, Options panel")]
    bool IsQuestionTypeSelected = false;

    [Comment("Is Question Type selected from QuestionDialogBox")]
    bool IsQuestionSelected = false;

    [Comment("Is Rearrange for Questions selected")]
    bool IsQuestionRearrangeSelected { get; set; } = false;

    [Comment("Is Rearrange for Options selected")]
    bool IsOptionRearrangeSelected { get; set; } = false;

    [Comment("What is the current QuestionCount for questions")]
    int QuestionCount = 0;

    [Comment("Selected question type like(True/False, Fill In the blanks etc!)")]
    static int? SelectedQuestionType { get; set; }

    [Comment("This property is being used to get the Selected Option for Options like checkbox and radio Button")]
    int SelectedOption { get; set; }

    MudForm form = null!;

    readonly ExamDto Exam = new();

    List<QuestionDto> Questions = new();

    QuestionDto model = new();

    readonly DialogOptions maxWidth = new() { MaxWidth = MaxWidth.Medium, FullWidth = true };

    List<QuestionDto> ComprehensionQuestions = new();

    #endregion

    #region Methods

    protected async override void OnInitialized()
    {
        model.ExamUniqueId = ExamId;

        await GetQuestions();
    }

    public async Task ImportQuestions()
    {
        DialogOptions options = new()
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseButton = true,
            Position = DialogPosition.Center,
            DisableBackdropClick = true
        };

        DialogParameters parameters = new() { ["ExamId"] = ExamId };

        var result = await Dialog.Show<ImportQuestionDialog>("", parameters, options).Result;

        if (result is not null)
        {
            await GetQuestions();
            StateHasChanged();

            // perform the action here
        }
    }

    /// <summary>
    /// This method is being used to Get all Questions by ExamId
    /// </summary>
    private async Task GetQuestions()
    {
        var questions = await QuestionsClient.GetAllByExamIdAsync(ExamId);

        Questions = questions.Questions;
    }

    /// <summary>
    /// This method is used to Rearrange Questions Display Order
    /// </summary>
    private void RearrangeQuestionsOrder()
    {
        IsQuestionTypeSelected = false;

        IsQuestionRearrangeSelected = !IsQuestionRearrangeSelected;
    }

    /// <summary>
    /// RearrangeOptionsOrder is use to Rearrange Options Display Order                                         
    /// </summary>
    private void RearrangeOptionsOrder() { IsOptionRearrangeSelected = !IsOptionRearrangeSelected; }

    /// <summary>
    /// This method is being used to update displayOrder of Questions
    /// </summary>
    private async void UpdateQuestionDisplayOrder()
    {
        var newDisplayOrders = Questions.Select(question => new DisplayOrderDto()
        {
            DisplayOrder = question.DisplayOrder,
            UniqueId = question.UniqueId ?? string.Empty
        }).ToList();

        if (await QuestionsClient.UpdateDisplayOrdersAsync(displayOrders: newDisplayOrders, examId: ExamId))
        {
            await GetQuestions();

            IsQuestionRearrangeSelected = false;
        }
    }

    private async void UpdateOptionDisplayOrder()
    {
        var newDisplayOrders = model.Options.Select(option => new DisplayOrderDto()
        {
            DisplayOrder = option.DisplayOrder,
            UniqueId = option.UniqueId ?? string.Empty
        }).ToList();

        if (await OptionsClient.UpdateDisplayOrdersAsync(displayOrders: newDisplayOrders, questionId: model.UniqueId))
        {
            IsOptionRearrangeSelected = false;

            model = await QuestionsClient.GetByIdAsync(model.UniqueId);

            StateHasChanged();
        }
    }

    /// <summary>
    /// This method is being used to Open QuestionTypeDialogBox where user can select the QuestuionType
    /// Also if user didnt save its exisitng quesiton before selection of new type, the question will be saved automatically
    /// </summary>
    /// <param name="options"></param>
    private async void OpenDialog(DialogOptions options)
    {
        ComprehensionQuestions.Clear();

        await AddNewQuestion();

        DialogParameters parameters = new() { ["ExamId"] = ExamId };

        var result = await Dialog.Show<ChooseQuestionTypeDialog>("", parameters, options).Result;

        if (result is null || result.Canceled)
        {
            IsQuestionTypeSelected = false;
            return;
        }
        IsQuestionTypeSelected = true;

        if (result.Data is int selectedType)
        {
            if (selectedType == QuestionTypes.ImportQuestions)
            {
                IsQuestionTypeSelected = false;
                await GetQuestions();
            }
            else
            {

                model.QuestionType_GeneralLookUpID = selectedType;

                SelectedQuestionType = selectedType;

                model.ExamUniqueId = ExamId;

                model.Options = CommonHelper.InitializeOptions(SelectedQuestionType);

                if (SelectedQuestionType == QuestionTypes.MultipleChoice || SelectedQuestionType == QuestionTypes.TrueFalse)
                    SelectedOption = 0;

                if (selectedType == QuestionTypes.Comprehension)
                {
                    /// For Comprehension question type we are saving it on the time of initialization since we need to save the parent question first
                    model.Name = "Comprehension";

                    await UpdateQuestionCount(model);

                    model.UniqueId = await QuestionsClient.CreateAsync(model);

                    Questions.Add(model);

                    model.Name = string.Empty;
                }

                IsQuestionSelected = false;
            }
        }

        StateHasChanged();
    }

    /// <summary>
    /// This method is being used to create or update question
    /// </summary>
    /// <returns></returns>
    private async Task AddNewQuestion()
    {
        if (!string.IsNullOrWhiteSpace(model.Name))
        {
            if (!string.IsNullOrWhiteSpace(model.UniqueId)) { await UpdateQuestion(model); }
            else { await CreateQuestion(); }

            model = new();

            await form.ResetAsync();

            StateHasChanged();
        }
    }

    /// <summary>
    /// This method is responsible for Creating a new Question and save it to database.
    /// </summary>
    private async Task CreateQuestion()
    {
        QuestionDto question = new()
        {
            ExamUniqueId = ExamId,
            QuestionType_GeneralLookUpID = SelectedQuestionType,
            Name = model.Name,
            Description = model.Description,
            Options = model.Options.ToList()
        };

        await SaveQuestions(question);
    }

    /// <summary>
    /// This method is being used to update the QuestionCount
    /// Once the question count is being setted, setting the QuestionCount to 0 so that it will not be used for next question
    /// </summary>
    private async Task UpdateQuestionCount(QuestionDto question)
    {
        if (Questions.Any()) { QuestionCount = await QuestionsClient.GetLargestDisplayOrderAsync(ExamId); }

        QuestionCount++;
        question.DisplayOrder = QuestionCount;

        //setting QuestionCount to 0 so that it will not be used for next question
        QuestionCount = 0;
    }

    /// <summary>
    /// This method is being used to Open the selected Question
    /// Based on the selected Question from left panel we are opening the Question on the right panel
    /// </summary>
    /// <param name="questionId"></param>
    private async Task OpenQuestion(string? questionId)
    {
        if (!IsQuestionRearrangeSelected)
        {
            model = await QuestionsClient.GetByIdAsync(questionId);

            SelectedQuestionType = model.QuestionType_GeneralLookUpID;

            if (SelectedQuestionType != QuestionTypes.DropDown && SelectedQuestionType != QuestionTypes.FillInTheBlank) { }

            if (SelectedQuestionType == QuestionTypes.Comprehension)
            {
                var questionCollection = await QuestionsClient.GetChildQuestionsWithIdAsync(questionId);

                ComprehensionQuestions = questionCollection.ToList();
            }

            IsQuestionSelected = true;

            if (SelectedQuestionType == QuestionTypes.MultipleChoice || SelectedQuestionType == QuestionTypes.TrueFalse)
                SelectedOption = model.Options.FindIndex(option => option.ISCorrect.Equals(true));

            IsQuestionTypeSelected = true;

            StateHasChanged();
        }
    }

    /// <summary>
    /// This method is responsible for removing the selected option from the question
    /// </summary>
    /// <param name="option"></param>
    private async void RemoveOption(OptionDto option)
    {
        if (!string.IsNullOrWhiteSpace(option.UniqueId))
            await OptionsClient.DeleteAsync(option.UniqueId);

        if (SelectedQuestionType != QuestionTypes.DropDown && SelectedQuestionType != QuestionTypes.FillInTheBlank) { }

        model.Options.Remove(option);

        StateHasChanged();
    }

    /// <summary>
    /// This method is being used to Add new Options to the Question.
    /// </summary>
    private void AddOptions()
    {
        switch (SelectedQuestionType)
        {
            case QuestionTypes.Matching:
                model.Options.Add(new OptionDto { Name = $"Choice [{model.Options.Count + 1}]", Match = $"Match [{model.Options.Count + 1}]", DisplayOrder = model.Options.Count is 0 ? 0 : (model.Options.Max(x => x.DisplayOrder) + 1), ISCorrect = true });
                break;

            case QuestionTypes.DragDropWithText:
                model.Options.Add(new OptionDto { Name = $"Blank [{model.Options.Count + 1}]", DisplayOrder = model.Options.Count is 0 ? 0 : (model.Options.Max(x => x.DisplayOrder) + 1), ISCorrect = true });
                break;

            default:
                model.Options.Add(new OptionDto { Name = "Option" + (model.Options.Count + 1).ToString(), DisplayOrder = model.Options.Count is 0 ? 0 : (model.Options.Max(x => x.DisplayOrder) + 1) });
                break;
        }

        StateHasChanged();
    }

    private void AddIncorrectMatch()
    {
        model.Options.Add(new OptionDto { Name = $"Incorrect Match [{model.Options.Count + 1}]", DisplayOrder = model.Options.Count is 0 ? 0 : (model.Options.Max(x => x.DisplayOrder) + 1), ISInCorrectMatch = true });

        StateHasChanged();
    }

    /// <summary>
    /// This method is being used to Save the Question
    /// </summary>
    /// <param name="question"></param>
    private async Task SaveQuestions(QuestionDto question)
    {
        if (model.Options.Count == 0 && SelectedQuestionType != QuestionTypes.Note && SelectedQuestionType != QuestionTypes.AudioVideo) { return; }

        if (SelectedQuestionType == QuestionTypes.MultipleChoice || SelectedQuestionType == QuestionTypes.TrueFalse)
        {
            foreach (var item in model.Options) { item.ISCorrect = false; }

            model.Options[SelectedOption].ISCorrect = true;
        }

        if (!string.IsNullOrWhiteSpace(model.UniqueId))
        {
            model.ExamUniqueId = ExamId;

            await UpdateQuestion(model);
        }

        if (!IsQuestionSelected)
        {
            if (SelectedQuestionType != QuestionTypes.Comprehension)
            {
                question.ExamUniqueId = ExamId;

                question.Name ??= "";

                await UpdateQuestionCount(question);

                var questionId = await QuestionsClient.CreateAsync(question);

                question.UniqueId = questionId;

                Questions.Add(question);
            }
        }

        model = new QuestionDto();

        IsQuestionTypeSelected = false;

        StateHasChanged();
    }

    /// <summary>
    /// This method is being used to Update the Question
    /// </summary>
    /// <param name="question"></param>
    /// <returns></returns>
    private async Task UpdateQuestion(QuestionDto question)
    {
        if (IsQuestionSelected)
        {
            if (CommandsReturnStatus.Updated.Equals(await QuestionsClient.UpdateAsync(questionId: question.UniqueId ?? "", question: question)))
            {
                var index = Questions.FindIndex(x => x.UniqueId == question.UniqueId);

                Questions[index] = question;
            }

            StateHasChanged();
        }
    }

    /// <summary>
    /// This method is used to remove the selected Question from exam
    /// </summary>
    /// <param name="question"></param>
    private async Task RemoveQuestion(QuestionDto question)
    {
        if (Questions.Any())
        {
            var questionToBeRemove = Questions.FirstOrDefault(x => x.UniqueId == question.UniqueId);

            if (questionToBeRemove != null)
            {
                Questions.Remove(questionToBeRemove);

                if (question.QuestionType_GeneralLookUpID == QuestionTypes.Comprehension)
                {
                    var questions = await QuestionsClient.GetChildQuestionsWithIdAsync(question.UniqueId);

                    var questionsCollection = questions.ToList();

                    foreach (var item in questionsCollection)
                    {
                        item.ParentQuestionId = null;
                        item.DisplayOrder = item.OldDisplayOrder ?? 0;
                        item.OldDisplayOrder = null;

                        await QuestionsClient.UpdateAsync(item.UniqueId, item);
                    }
                }

                var commandsReturnStatus = await QuestionsClient.DeleteAsync(question.UniqueId);

                if (commandsReturnStatus == CommandsReturnStatus.Deleted.ToString()) { IsQuestionTypeSelected = false; }
            }
            else { IsQuestionTypeSelected = false; }

            QuestionCount = 0;

            await GetQuestions();

            StateHasChanged();
        }

        model = new QuestionDto();
    }

    public async Task UploadImages(InputFileChangeEventArgs e, OptionDto? option, bool imageForQuestion = true)
    {
        var fileDetail = GetFileDetail(e);

        using var stream = e.File.OpenReadStream(maxAllowedSize: 10000 * 10000);
        await stream.ReadAsync(fileDetail.MetaData.FileBytes);

        if (imageForQuestion) { model.Media = await FilesClient.UploadAsync(fileDetail); }
        else if (option is not null) { option.Media = await FilesClient.UploadAsync(fileDetail); }
    }

    private static FileDetailDto GetFileDetail(InputFileChangeEventArgs e)
    {
        return new FileDetailDto()
        {
            MetaData = new()
            {
                FileBytes = new byte[e.File.Size],
                FileName = e.File.Name,
                FileSize = e.File.Size,
                FileType = e.File.ContentType
            },
            Path = FileDirectory.OptionImage
        };
    }

    private async Task GetQuestionsForComprehension(DialogOptions options)
    {
        var parameters = new DialogParameters<AssignQuestionsDialogbox> { { x => x.ExamId, ExamId }, { x => x.QuestionId, model.UniqueId } };

        var result = await Dialog.Show<AssignQuestionsDialogbox>("Select Questions", parameters, options).Result;

        if (result is null || result.Canceled) { return; }

        var selectedQuestions = (List<QuestionDto>)result.Data;

        ComprehensionQuestions.AddRange(selectedQuestions);

        await GetQuestions();

        StateHasChanged();
    }

    private async Task RemoveComprehensionQuestion(QuestionDto question)
    {
        question.DisplayOrder = question.OldDisplayOrder ?? 0;
        question.OldDisplayOrder = null;
        question.ParentQuestionId = null;

        var result = await QuestionsClient.UpdateAsync(question.UniqueId, question);

        if (result == CommandsReturnStatus.Updated)
        {
            ComprehensionQuestions.Remove(question);

            Questions.Add(question);
        }

        await GetQuestions();

        StateHasChanged();
    }

    void ClearImage() => model.Media = null;

    #endregion
}
