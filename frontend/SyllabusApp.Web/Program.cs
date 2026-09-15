using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SyllabusApp.Web;
using SyllabusApp.Web.Auth;
using SyllabusApp.Web.Services;
using SyllabusApp.Web.Services.ApiClients;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7242/";

builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddTransient<AuthorizationMessageHandler>();
builder.Services.AddHttpClient("Api", client => client.BaseAddress = new Uri(apiBaseUrl))
    .AddHttpMessageHandler<AuthorizationMessageHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Api"));

builder.Services.AddScoped<LocationApiClient>();
builder.Services.AddScoped<FacultyApiClient>();
builder.Services.AddScoped<FieldOfStudyApiClient>();
builder.Services.AddScoped<CourseApiClient>();
builder.Services.AddScoped<StudentGroupApiClient>();
builder.Services.AddScoped<SyllabusApiClient>();
builder.Services.AddScoped<LearningOutcomeApiClient>();
builder.Services.AddScoped<LiteratureApiClient>();
builder.Services.AddScoped<WorkloadItemApiClient>();

await builder.Build().RunAsync();
