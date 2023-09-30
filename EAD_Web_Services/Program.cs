using EAD_Web_Services.Models.TrainModel;
using EAD_Web_Services.Services.TrainService;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);


// Train services
builder.Services.Configure<TrainStoreDatabaseSettings>(
       builder.Configuration.GetSection(nameof(TrainStoreDatabaseSettings)));

builder.Services.AddSingleton<ITrainStoreDatabaseSettings>(sp =>
       sp.GetRequiredService<IOptions<TrainStoreDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
  new MongoClient(builder.Configuration.GetValue<string>("TrainStoreDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<ITrainService, TrainService>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
