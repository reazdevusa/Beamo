using Beamo.Core.Services.Database;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddSingleton<IDatabaseService>(sp =>
    new DatabaseService(
        Path.Combine(AppContext.BaseDirectory, "beamo.db"),
        sp.GetRequiredService<ILogger<DatabaseService>>()
    ));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Beamo API",
        Version = "v1",
        Description = "Backend API for the Beamo networking app — contacts, exchanges, referrals & notifications.",
        Contact = new OpenApiContact
        {
            Name = "Reaz Ahmed",
            Url = new Uri("https://reazlab.com")
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Correct order
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Beamo API v1");
    c.RoutePrefix = string.Empty;
});

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
