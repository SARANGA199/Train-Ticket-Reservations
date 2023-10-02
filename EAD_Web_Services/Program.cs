using EAD_Web_Services.DatabaseConfiguration;
using EAD_Web_Services.Models.TrainModel;
using EAD_Web_Services.Services.RequestAgentService;
using EAD_Web_Services.Services.ReservationService;
using EAD_Web_Services.Services.TrainService;
using EAD_Web_Services.Services.UserService;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);


//services configuration
builder.Services.Configure<DatabaseSettings>(
       builder.Configuration.GetSection(nameof(DatabaseSettings)));

builder.Services.AddSingleton<IDatabaseSettings>(sp =>
       sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
  new MongoClient(builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString")));

builder.Services.AddScoped<ITrainService, TrainService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddScoped<IRequestAgentService, RequestAgentService>();


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
