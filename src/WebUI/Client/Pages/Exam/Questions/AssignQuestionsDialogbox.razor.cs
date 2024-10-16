using CheetahExam.WebUI.Shared.Common.Models.Exams;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static CheetahExam.WebUI.Shared.Constants.Constant;
using static CheetahExam.WebUI.Shared.Enums.Enum;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions;

public partial class AssignQuestionsDialogbox
{
    #region Fields

    [Parameter] public string? ExamId { get; set; }

    [Parameter] public string? QuestionId { get; set; }

    [Inject] public IQuestionsClient QuestionsClient { get; set; } = null!;

    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; } = null!;

    private List<QuestionDto> Items = new();

    private readonly List<QuestionDto> selectedItems = new();

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        var questionCollection = await QuestionsClient.GetAllByExamIdAsync(ExamId ?? "");

        // Filter out questions with (Comprehension, Note, AudioVideo, RecordAudioVideo, Document) QuestionTypes, OldDisplayOrder is null or 0 and ParentQuestionId is null
        Items = questionCollection.Questions
        .Where(question =>
            (question.QuestionType_GeneralLookUpID != QuestionTypes.Comprehension) &&
            (question.QuestionLevelType_GeneralLookUpID != QuestionTypes.Note) &&
            (question.QuestionLevelType_GeneralLookUpID != QuestionTypes.AudioVideo) &&
            (question.QuestionLevelType_GeneralLookUpID != QuestionTypes.RecordAudioVideo) &&
            (question.QuestionLevelType_GeneralLookUpID != QuestionTypes.Document) &&
            ((question.OldDisplayOrder == null || question.OldDisplayOrder == 0) || question.ParentQuestionId == null))
        .ToList();
    }

    private async Task SelectedItemsChanged(HashSet<QuestionDto> selectedQuestions)
    {
        int currentIndex = 1;

        foreach (var question in selectedQuestions.Where(item => !selectedItems.Contains(item)))
        {
            question.OldDisplayOrder = question.DisplayOrder;
            question.DisplayOrder = currentIndex++;
            question.ParentQuestionId = QuestionId;

            var result = await QuestionsClient.UpdateAsync(question.UniqueId, question);

            if (result == CommandsReturnStatus.Updated) { selectedItems.Add(question); }
        }
    }

    void Cancel() => MudDialog.Cancel();

    void AssignQuestions() => MudDialog.Close(selectedItems);

    #endregion
}
