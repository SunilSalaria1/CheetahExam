namespace CheetahExam.Application.Common.Services.Data;

public interface IApplicationDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    DbSet<Course> Courses { get; }

    DbSet<Exam> Exams { get; }

    DbSet<ExamResultOption> ExamResultOptions { get; }

    DbSet<Question> Questions { get; }

    DbSet<Option> Options { get; }

    DbSet<Media> Media { get; }

    DbSet<GeneralLookUp> GeneralLookUps { get; }

    DbSet<Company> Companies { get; }

    DbSet<User> Users { get; }

    DbSet<Role> Roles { get; }

    DbSet<UserRoleMapper> UserRoleMappers { get; }

    DbSet<Font> Fonts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
