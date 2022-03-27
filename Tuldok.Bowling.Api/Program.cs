using Microsoft.EntityFrameworkCore;
using Tuldok.Bowling.Api.Policies;
using Tuldok.Bowling.Data.Entities;
using Tuldok.Bowling.Repo.Data;
using Tuldok.Bowling.Service;
using Tuldok.Bowling.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connStr = builder.Configuration.GetConnectionString("LocalDb");
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(connStr);
});

builder.Services.AddScoped<IRepository<Game>, DataRepository<Game>>();
builder.Services.AddScoped<IRepository<Frame>, DataRepository<Frame>>();
builder.Services.AddScoped<IRepository<Shot>, DataRepository<Shot>>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IFrameService, FrameService>();
builder.Services.AddScoped<IShotService, ShotService>();
builder.Services.AddScoped<IScoreService, ScoreService>();
builder.Services.AddScoped<IBowlingService, BowlingService>();

var app = builder.Build();

// Migrate on startup
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<DataContext>();
    ctx.Database.Migrate();
}

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
