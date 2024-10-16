using CheetahExam.WebUI.Shared.Common.Models;
using CheetahExam.WebUI.Shared.Common.Models.Exams;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions.Templates;

public partial class DropdownTemplate
{
    QuestionDto Question { get; set; } = null!;

    int? SelectedOption { get; set; }

    MudForm Form { get; set; } = null!;

    KeyValueCollection Options { get; set; } = new();

    protected override void OnInitialized()
    {
        Question = new()
        {
            Name = "Select the best opening question from the dropdown to use when approaching a customer:",
            Options = new()
            {
                new() 
                {
                    Name = "What's going on?",
                    ISCorrect = true
                },
                new()
                {
                    Name = "Is there something in particular you were looking for today?",
                    ISCorrect = false
                },
                new()
                {
                    Name = "What is the weather like outside?",
                    ISCorrect = false
                },
                new()
                {
                    Name = "Do you want anything?",
                    ISCorrect = false
                }
            }
        };
        
        for(int i=0; i<Question.Options.Count; i++)
        {
            Options.KeyValues.Add(new KeyValue { Key = i, Value = Question.Options[i].Name });
        }
    }

    void OnOptionSelected(int? selectedOption) { SelectedOption = selectedOption; }
}
