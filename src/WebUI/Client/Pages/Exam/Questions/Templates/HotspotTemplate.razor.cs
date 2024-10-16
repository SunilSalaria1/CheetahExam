using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions.Templates;

public partial class HotspotTemplate
{
    public QuestionDto Model { get; set; } = new() 
    {
        Name = "Pinpoint South America on the World Map.",
        Media = new() { Url = "https://cdn.britannica.com/98/152298-050-8E45510A/Cheetah.jpg" }
    };
}
