using Blazored.LocalStorage;
using CheetahExam.WebUI.Client;
using CheetahExam.WebUI.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
//using CheetahExam.WebUI.Client.Authorization;
//using CheetahExam.WebUI.Shared.Authorization;
using MudBlazor.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("CheetahExam.WebUI.ServerAPI"));

builder.Services.AddHttpClient("CheetahExam.WebUI.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)); //.AddHttpMessageHandler<>();
//.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddSingleton(services => (IJSInProcessRuntime)services.GetRequiredService<IJSRuntime>());

builder.Services.AddScoped<IAccountClient, AccountClient>();
builder.Services.AddScoped<IExamsClient, ExamsClient>();
builder.Services.AddScoped<IQuestionsClient, QuestionsClient>();
builder.Services.AddScoped<IOptionsClient, OptionsClient>();
builder.Services.AddScoped<IGeneralLookUpsClient, GeneralLookUpsClient>();
builder.Services.AddScoped<IFilesClient, FilesClient>();
builder.Services.AddScoped<ICompanyClient, CompanyClient>();
builder.Services.AddScoped<IUsersClient, UsersClient>();
builder.Services.AddScoped<IRolesClient, RolesClient>();
builder.Services.AddScoped<IExamImportExportService, ExamImportExportService>();
builder.Services.AddScoped<IValidateExcelService, ValidateExcelService>();

builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());

//builder.Services
//    .AddApiAuthorization();
//.AddAccountClaimsPrincipalFactory<CustomAccountClaimsPrincipalFactory>();

//builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
//builder.Services.AddSingleton<IAuthorizationPolicyProvider, FlexibleAuthorizationPolicyProvider>();

//builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorizationCore();

builder.Services.AddBlazoredLocalStorage();

// NOTE: https://github.com/khellang/Scrutor/issues/180
//builder.Services.Scan(scan => scan
//    .FromAssemblyOf<IWeatherForecastClient>()
//    .AddClasses()
//    .AsImplementedInterfaces()
//    .WithScopedLifetime());

// mudblazor
builder.Services.AddMudServices();

await builder.Build().RunAsync();
