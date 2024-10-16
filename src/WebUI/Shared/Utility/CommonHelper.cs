using CheetahExam.WebUI.Shared.Common.Models;
using CheetahExam.WebUI.Shared.Common.Models.Exams;
using static CheetahExam.WebUI.Shared.Enums.Enum;

namespace CheetahExam.WebUI.Shared.Utility;

public static class CommonHelper
{
    #region Methods

    public static string GetAlphabetByIndex(int index)
    {
        if (index >= 0 && index <= 25)
        {
            char alphabet = (char)('A' + index);
            return alphabet.ToString();
        }
        else { return "Z"; /* because we are not sure what to display after 26 alphabet which is Z */ }
    }

    public static int GetIndexByAlphabet(char alphabet)
    {
        if (alphabet >= 'A' && alphabet <= 'Z')
        {
            int index = alphabet - 'A';
            return index;
        }
        else { return -1; }
    }

    public static void ParseDisplayOrder(string userInput, OptionDto option, QuestionDto question)
    {
        char inputChar = userInput.ToUpper()[0];
        int displayOrder = GetIndexByAlphabet(inputChar);

        if (displayOrder != -1)
        {
            var existingModel = question.Options.FirstOrDefault(x => x.DisplayOrder == displayOrder);

            option.DisplayOrder = displayOrder;
        }
    }

    /// <summary>
    /// This method will give some default options based on the selected QuestionType
    /// </summary>
    /// <returns> List of Options</returns>
    public static List<OptionDto> InitializeOptions(int? selectedQuestionType)
    {
        switch (selectedQuestionType)
        {
            case QuestionTypes.TrueFalse:
                return Enumerable.Range(0, 2).Select(index => new OptionDto() { Name = index == 0 ? "True" : "False", DisplayOrder = index }).ToList();

            case QuestionTypes.FillInTheBlank:
                return Enumerable.Range(0, 0).Select(index => new OptionDto() { Name = $"Answer for blank [{index + 1}]", DisplayOrder = index }).ToList();

            case QuestionTypes.Note:
            case QuestionTypes.AudioVideo:
                return Enumerable.Range(0, 0).Select(index => new OptionDto() { Name = $"Answer for blank [{index}]", DisplayOrder = index }).ToList();

            case QuestionTypes.Matching:
                return Enumerable.Range(0, 2).Select(index => new OptionDto() { Name = $"Choice [{index}]", Match = $"Match [{index}]", DisplayOrder = index }).ToList();

            case QuestionTypes.DragDropWithText:
                return Enumerable.Range(0, 0).Select(index => new OptionDto() { Name = $"Blank [{index + 1}]", DisplayOrder = index, ISCorrect = true }).ToList();

            default:
                return Enumerable.Range(0, 4).Select(index => new OptionDto() { Name = $"Option {index + 1}", DisplayOrder = index }).ToList();
        }
    }

    public static void AddBlank(QuestionDto model)
    {
        model.Name += " [Blank] ";

        if (model.Options.Any())
        {
            int maxDisplayOrder = model.Options.Max(option => option.DisplayOrder);

            model.Options.Add(new OptionDto { Name = "Answer for Blank[" + (maxDisplayOrder + 1).ToString() + "]", DisplayOrder = (maxDisplayOrder + 1) });
        }
        else { model.Options.Add(new OptionDto { Name = "Answer for Blank[" + (model.Options.Count() + 1).ToString() + "]", DisplayOrder = 0 }); }
    }

    public static void AddOptions(QuestionDto model, FileDetailCollectionDto optionFileDetailCollection, int selectedQuestionType)
    {
        model.Options.Add(new OptionDto { Name = "Option" + (model.Options.Count + 1).ToString(), DisplayOrder = model.Options.Count is 0 ? 0 : (model.Options.Max(x => x.DisplayOrder) + 1) });

        if (selectedQuestionType is not QuestionTypes.DropDown and QuestionTypes.FillInTheBlank) { optionFileDetailCollection.QuestionOptionsFile.Add(null); }
    }

    public static List<ExamResultOptionDto> GenerateLetterGradingOptions()
    {
        return new List<ExamResultOptionDto>
        {
            new(){ OptionName = "Grade A", MinPercentage = 90.0 },
            new(){ OptionName = "Grade B", MinPercentage = 80.0 },
            new(){ OptionName = "Grade C", MinPercentage = 70.0 },
            new(){ OptionName = "Grade D", MinPercentage = 60.0 },
            new(){ OptionName = "Grade E", MinPercentage = 50.0 },
        };
    }

    public static List<ExamResultOptionDto> GenerateGoodExcellentOptions()
    {
        return new List<ExamResultOptionDto>
        {
            new(){ OptionName = "Excellent", MinPercentage = 90.0 },
            new(){ OptionName = "Very Good", MinPercentage = 80.0 },
            new(){ OptionName = "Good", MinPercentage = 70.0 },
            new(){ OptionName = "Fair", MinPercentage = 60.0 },
            new(){ OptionName = "Needs Improvement", MinPercentage = 0.0 },
        };
    }

    #region Exam Import Export

    public static int GetQuestionTypeFromValue(string value)
    {
        switch (value)
        {
            case "True / False":
                return QuestionTypes.TrueFalse;
            case "Multiple Choice":
                return QuestionTypes.MultipleChoice;
            case "Checkboxes":
                return QuestionTypes.CheckBox;
            default:
                return 0;
        }
    }

    /// <summary>
    /// This method will only be used for (MultipleChoice, CheckBox, TrueFalse) question types
    /// </summary>
    /// <param name="options"></param>
    /// <param name="value"></param>
    public static void SetCorrectOption(List<OptionDto> options, string? value)
    {
        var option = options.FirstOrDefault(option => (GetAlphabetByIndex(option.DisplayOrder)[0] == value[0]));
        if (option is not null) { option.ISCorrect = true; }
    }

    public static string GetQuestionTypeWithId(int? id) 
    {
        switch (id)
        {
            case 1:
               return "Checkboxes";
            case 2:
                return "Radio Button";
            case 3: 
                return "True / False";
            default:
                return "Unknown";
        }
    }

    #endregion

    #endregion
}
