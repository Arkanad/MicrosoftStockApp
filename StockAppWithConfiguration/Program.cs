using StockAppWithConfiguration.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<FinnhubService>();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();
app.UseRouting();

app.Run();
