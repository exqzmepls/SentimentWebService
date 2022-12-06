using SentimentWebService.Clients.SentimentPrediction;
using SentimentWebService.Clients.SentimentPrediction.Sentiment140;
using SentimentWebService.Clients.Youtube;
using SentimentWebService.Services.YoutubeComments;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient("sentiment140", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddSingleton<IYoutubeClient>(new YoutubeClient("AIzaSyB5tpbr8Lt1KRevskK-K3LwP3wAmGanGUI"));
builder.Services.AddSingleton<ISentimentPredictionClient, Sentiment140Client>();
builder.Services.AddSingleton<ISentimentService, SentimentService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
