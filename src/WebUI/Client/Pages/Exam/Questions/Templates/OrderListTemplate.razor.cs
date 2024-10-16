using CheetahExam.WebUI.Shared.Common.Models.Exams;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions.Templates;

public partial class OrderListTemplate
{
    private MudForm Form { get; set; } = null!;

    private MudDropContainer<OptionDto> _container = null!;

    public QuestionDto Question { get; set; } = null!;

    protected override void OnInitialized()
    {
        Question = new()
        {
            Name = "Arrange these countries from smallest to largest in terms of land area.",
            Options = new()
        {
            new() { Name = "Mexico", Index = 1 },
            new() { Name = "New Zealand", Index = 2 },
            new() { Name = "India", Index = 3 },
            new() { Name = "Pakistan", Index = 4 }
        }
        };
    }

    private void ItemUpdated(MudItemDropInfo<OptionDto> dropItem)
    {
        var draggedOption = dropItem.Item;

        Question.Options.Remove(draggedOption);

        Question.Options.Insert(dropItem.IndexInZone, draggedOption);

        for (int i = 0; i < Question.Options.Count; i++)
        {
            Question.Options[i].Index = i + 1;
        }

        StateHasChanged();
    }
}
