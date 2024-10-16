using CheetahExam.Application.Common.Services.Data;
using CheetahExam.Infrastructure.Data.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CheetahExam.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(
        DbContextOptions options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options) //, operationalStoreOptions)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    #region DbSets

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<Exam> Exams => Set<Exam>();

    public DbSet<ExamResultOption> ExamResultOptions => Set<ExamResultOption>();

    public DbSet<Question> Questions => Set<Question>();

    public DbSet<Option> Options => Set<Option>();

    public DbSet<Media> Media => Set<Media>();

    public DbSet<GeneralLookUp> GeneralLookUps => Set<GeneralLookUp>();

    public DbSet<Company> Companies => Set<Company>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Font> Fonts => Set<Font>();

    public DbSet<UserRoleMapper> UserRoleMappers => Set<UserRoleMapper>();

    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder
            .AddInterceptors(_auditableEntitySaveChangesInterceptor);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
