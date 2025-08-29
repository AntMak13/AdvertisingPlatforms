using AdvertisingPlatforms.Services;

var builder = WebApplication.CreateBuilder(args);

// in-memory service
builder.Services.AddSingleton<IAdvertisingService, AdvertisingService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// app.UseHttpsRedirection(); // ?

app.Run();


/*
 * Для запуска
 * dotnet run --urls "http://localhost:5000;https://localhost:5001"
 * 
*/