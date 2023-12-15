using Hangfire;
using Hangfire.Console;
using Hangfire.HttpJob;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(c => c
    .UseMemoryStorage()
    .UseConsole()
    .UseHangfireHttpJob(new HangfireHttpJobOptions()
    {
        DashboardTitle = "",
    })
);
builder.Services.AddHangfireServer();

var app = builder.Build();

#region 全球化(語系)設定
var supportedCultures = new[]
{
    new CultureInfo("es"),
};
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es"),
    // Formatting numbers, dates, etc.
    SupportedCultures = supportedCultures,
    // UI strings that we have localized.
    SupportedUICultures = supportedCultures
});
#endregion

app.UseHangfireDashboard();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
