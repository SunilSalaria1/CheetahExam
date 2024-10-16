using Microsoft.EntityFrameworkCore;

namespace CheetahExam.Infrastructure.Data;

public class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _context;
    private const string AdministratorRole = "Administrator";
    private const string SuperAdminRole = "Super Admin";
    private const string AdminRole = "Admin";
    private const string DefaultPassword = "Password123!";

    public ApplicationDbContextInitialiser(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        await InitialiseWithMigrationsAsync();
    }

    private async Task InitialiseWithDropCreateAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
    }

    private async Task InitialiseWithMigrationsAsync()
    {
        if (_context.Database.IsSqlServer())
        {
            await _context.Database.MigrateAsync();
        }
        else
        {
            await _context.Database.EnsureCreatedAsync();
        }
    }

    public async Task SeedAsync()
    {
        await SeedUserDataAsync();
        await SeedDataAsync();
        await SeedExamDataAsync();
    }

    #region Seeding Admin Information

    private async Task SeedUserDataAsync()
    {
        if (await _context.Users.AnyAsync() || await _context.Roles.AnyAsync())
        {
            return;
        }

        var roles = new[]
            {
                new Role(){ Name = AdministratorRole, NormalizedName = AdministratorRole.ToUpperInvariant() },
                new Role(){ Name = SuperAdminRole, NormalizedName = SuperAdminRole.ToUpperInvariant() },
                new Role(){ Name = AdminRole, NormalizedName = AdminRole.ToUpperInvariant()},
                //new Role(){ Name = StudentRole, NormalizedName = StudentRole.ToUpperInvariant() }
            };

        var users = new[]
            {
                new User()
                {
                    FirstName = "Michelle",
                    LastName = "LaBrosse",
                    UserName = "michelle.labrosse@localhost.com",
                    Email = "michelle.labrosse@localhost.com",
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword(DefaultPassword)
                },
                new User()
                {
                    FirstName = "Admin",
                    LastName = "User",
                    UserName = "admin.user@localhost.com",
                    Email = "admin.user@localhost.com",
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword(DefaultPassword)
                },

                #region We may need it later please dont remove this

            #endregion
        };

        await _context.UserRoleMappers.AddRangeAsync(new[]
                    {
                new UserRoleMapper(){ UserRoleMapper_UserID = 1, UserRoleMapper_RoleID = 1, UserRoleMapper_User = users[0], UserRoleMapper_Role = roles[0] },
                new UserRoleMapper(){ UserRoleMapper_UserID = 2, UserRoleMapper_RoleID= 2, UserRoleMapper_User = users[1], UserRoleMapper_Role= roles[2] },
            });

        await _context.Roles.AddAsync(
               new Role() { Name = SuperAdminRole, NormalizedName = SuperAdminRole.ToUpperInvariant() });


        await _context.SaveChangesAsync();

    }

    #endregion

    private async Task SeedDataAsync()
    {
        if (await _context.GeneralLookUps.AnyAsync())
        {
            return;
        }

        List<GeneralLookUp> generalLookUps = new()
        {
            new GeneralLookUp { Type = "Question_Type", Value = "MultipleChoice", DisplayOrder = 1, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Question_Type", Value = "Checkbox", DisplayOrder = 2, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Question_Type", Value = "TrueFalse", DisplayOrder = 3, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Question_Type", Value = "FillInTheBlank", DisplayOrder = 4, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Question_Type", Value = "Essay", DisplayOrder = 5, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Question_Type", Value = "HotSpots", DisplayOrder = 6, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Question_Type", Value = "DragAndDrop", DisplayOrder = 7, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Question_Type", Value = "Matching", DisplayOrder = 8, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Discipline_Type", Value = "Project Management", DisplayOrder = 1, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Discipline_Type", Value = "Leadership", DisplayOrder = 2, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Discipline_Type", Value = "Teams", DisplayOrder = 3, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Scope", DisplayOrder = 1, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Budget", DisplayOrder = 2, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Schedule", DisplayOrder = 3, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Risk", DisplayOrder = 4, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Quality", DisplayOrder = 5, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Stakeholder", DisplayOrder = 6, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Communication", DisplayOrder = 7, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Integration", DisplayOrder = 8, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Governance", DisplayOrder = 9, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Planning", DisplayOrder = 10, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Execution", DisplayOrder = 11, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Control", DisplayOrder = 12, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Closeout", DisplayOrder = 13, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Initiation", DisplayOrder = 14, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Agile Projects", DisplayOrder = 15, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Leadership", DisplayOrder = 16, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Business", DisplayOrder = 17, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Environment", DisplayOrder = 18, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Category_Type", Value = "Project Management Processes", DisplayOrder = 19, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "QuestionLevel_Type", Value = "Easy", DisplayOrder = 1, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "QuestionLevel_Type", Value = "Hard", DisplayOrder = 2, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "QuestionLevel_Type", Value = "Medium", DisplayOrder = 3, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "QuestionLevel_Type", Value = "Tough",DisplayOrder = 4, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Media_Type", Value = "Image", DisplayOrder = 1, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Media_Type", Value = "Video", DisplayOrder = 2, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Media_Type", Value = "Doc", DisplayOrder = 3, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Media_Type", Value = "Audio", DisplayOrder = 4, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Result_Type", Value = "Pass or Fail", DisplayOrder = 1, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Result_Type", Value = "Letter Grading", DisplayOrder = 2, ISActive = true, ISArchive = false},
            new GeneralLookUp { Type = "Result_Type", Value = "Good or Excellent", DisplayOrder = 3, ISActive = true, ISArchive = false}
        };

        await _context.GeneralLookUps.AddRangeAsync(generalLookUps);
        await _context.SaveChangesAsync();
    }
    private async Task SeedExamDataAsync()
    {
        if (await _context.Fonts.AnyAsync())
        {
            return;
        }

        List<Font> fonts = new()
        {
            new() { Name = "Arial", Family = "Arial" },
            new() { Name = "Courier New", Family = "Courier New" },
            new() { Name = "Georgia", Family = "Georgia" },
            new() { Name = "Lucida Sans Unicode", Family = "Lucida Sans Unicode" },
            new() { Name = "Tahoma", Family = "Tahoma" },
            new() { Name = "Times New Roman", Family = "Times New Roman" },
            new() { Name = "Trebuchet MS", Family = "Trebuchet MS" },
            new() { Name = "Verdana", Family = "Verdana" }
        };

        await _context.Fonts.AddRangeAsync(fonts);

        await _context.SaveChangesAsync();
    }
}
