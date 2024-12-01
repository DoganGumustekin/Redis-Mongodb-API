using StackExchange.Redis;
using TrainingRedisAPI.Models;
using TrainingRedisAPI.RedisImplementations;
using TrainingRedisAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("PostServiceSettings"));

builder.Services.AddSingleton<PostService>();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp => sp.ConfigureRedis(builder.Configuration));

builder.Services.AddSingleton<IRedisEntityRepository, RedisEntityRepositoryBase>();



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
