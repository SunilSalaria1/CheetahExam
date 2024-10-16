//using Azure.Identity;
using CheetahExam.Application.Common.Services.Account;
using CheetahExam.Application.Common.Services.File;
using CheetahExam.Infrastructure.Data;
using CheetahExam.WebUI.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//// Use key vault if configured
//var keyVaultName = builder.Configuration["KeyVaultName"];
//if (!string.IsNullOrWhiteSpace(keyVaultName))
//{
//    builder.Configuration.AddAzureKeyVault(
//        new Uri($"https://{keyVaultName}.vault.azure.net"),
//        new DefaultAzureCredential());
//}

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Adding Authentication  
builder.Services.AddAuthentication(
    options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ClockSkew = TimeSpan.FromSeconds(15),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });

builder.Services.AddAuthentication();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<IAccountService, AccountService>();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddOpenApiDocument(configure =>
{
    configure.Title = "CleanArchitecture API";
    configure.AddSecurity("JWT", Enumerable.Empty<string>(),
        new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Type into the textbox: Bearer {your JWT token}."
        });

    configure.OperationProcessors.Add(
        new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

builder.Services.AddApplicationInsightsTelemetry();
var app = builder.Build();

// Initialise and seed the database
using (var scope = app.Services.CreateScope())
{
    try
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database initialisation.");

        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();

app.UseStaticFiles();

app.UseSwaggerUi3(configure =>
{
    configure.DocumentPath = "/api/v1/openapi.json";
});

app.UseReDoc(configure =>
{
    configure.Path = "/redoc";
    configure.DocumentPath = "/api/v1/openapi.json";
});

app.UseRouting();

app.UseCors(cors => cors
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin()
    );

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
