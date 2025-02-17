using MudBlazor.Services;
using StudyPlannerApplication.App.Components;
using StudyPlannerApplication.App.Services;
using StudyPlannerApplication.Database.EFAppDbContextModels;
using StudyPlannerApplication.Domain.Features.Exam;
using StudyPlannerApplication.Domain.Features.UserManagement.Profile;
using StudyPlannerApplication.Domain.Features.UserManagement.SignIn;
using StudyPlannerApplication.Domain.Features.UserManagement.UserRegistration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
//builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

#region DbService
var connectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(connectionString),
ServiceLifetime.Transient,
ServiceLifetime.Transient);

#endregion

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddScoped<IInjectService, InjectService>();
//builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddScoped<SignInService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<ExamService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
