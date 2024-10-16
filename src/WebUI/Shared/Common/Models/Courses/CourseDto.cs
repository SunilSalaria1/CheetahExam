using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Common.Models.Courses;

public class CourseDto
{
    public string Name { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? NumberOfStudents { get; set; }

    public string? CourseCategory { get; set; }

    public double? Amount { get; set; }

    public List<ExamDto> Exams { get; set; } = new();
}
