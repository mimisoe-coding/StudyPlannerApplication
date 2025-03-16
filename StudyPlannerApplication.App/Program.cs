using Serilog;
using StudyPlannerApplication.Domain.Features.LiveChat;
using StudyPlannerApplication.Domain.Hubs;
using System.Reflection;

string logDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty, "log");
Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
         .WriteTo.File(Path.Combine(logDirectory, "stp-.txt"), rollingInterval: RollingInterval.Hour)
        .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
// Add Serilog to logging
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
//builder.Services.AddServerSideBlazor();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", corsPolicyBuilder =>
        corsPolicyBuilder.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyHeader());
});
builder.Services.AddMudServices();

#region DbService
var connectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(connectionString),
ServiceLifetime.Transient,
ServiceLifetime.Transient);

builder.Services.AddScoped<DapperService>(x => new DapperService(connectionString));

#endregion

builder.Services.AddScoped<IInjectService, InjectService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddSingleton<NotificationStateContainer>();
builder.Services.AddScoped<SignInService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<ExamService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ReminderService>();
builder.Services.AddScoped<ChangePasswordService>();
builder.Services.AddScoped<LiveChatService>();
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
app.MapHub<LiveChatHub>("/liveChatHub");
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
