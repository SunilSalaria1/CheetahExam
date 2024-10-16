using CheetahExam.Domain.Common;

namespace CheetahExam.Domain.Entities;

public class Course : BaseAuditableEntity
{
    public int? Courses_UserID { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? NumberOfStudents { get; set; }

    public string? CourseCategory { get; set; }

    public double? Amount { get; set; }

    public virtual User? User { get; set; }
}
