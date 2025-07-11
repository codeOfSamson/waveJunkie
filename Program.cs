using SurfForecastApi.Services;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<XweatherSettings>(
    builder.Configuration.GetSection("Xweather"));

builder.Services.AddHttpClient<XweatherClient>(client =>
{
    var baseUrl = builder.Configuration["Xweather:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl!);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
