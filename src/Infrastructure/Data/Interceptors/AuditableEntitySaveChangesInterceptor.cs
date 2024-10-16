using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using CheetahExam.Domain.Common;
using CheetahExam.Application.Common.Services.Account;

namespace CheetahExam.Infrastructure.Data.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    #region Fields

    private readonly IAccountService _accountService;

    #endregion

    #region Ctor

    public AuditableEntitySaveChangesInterceptor(IAccountService accountService)
    {
        _accountService = accountService;
    }

    #endregion

    #region Method

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        AuditEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        AuditEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void AuditEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _accountService.CurrentUser();
                entry.Entity.CreatedUtc = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Added ||
                entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedBy = _accountService.CurrentUser();
                entry.Entity.LastModifiedUtc = DateTime.UtcNow;
            }
        }
    }

    #endregion
}
