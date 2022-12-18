using Core.Clients.SentimentPrediction;
using Core.Clients.SentimentPrediction.Sentiment140;
using Core.Clients.Youtube;
using Core.Repositories.Analysis;
using Core.Repositories.Comment;
using Core.Services.YoutubeComments;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using SentimentWebService.Tools.Youtube;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<Context>(options =>
{
    var connectionString = configuration.GetConnectionString("db");
    options.UseNpgsql(connectionString);
});
builder.Services.AddHttpClient("sentiment140", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddSingleton<IYoutubeClient>(new YoutubeClient(configuration.GetValue<string>("GoogleApiKey")));
builder.Services.AddSingleton<ISentimentPredictionClient, Sentiment140Client>();
builder.Services.AddSingleton<IUrlParser, RegexUrlParser>();
builder.Services.AddScoped<IAnalysisRepository, AnalysisRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ISentimentService, SentimentService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
