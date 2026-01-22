using API.Data;
using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite("Data Source=tasks.db"));

builder.Services.AddScoped<OpenAiPriorityService>();
builder.Services.AddControllers();
builder.Services.AddCors(o =>
{
    o.AddPolicy("blazor", p =>
        p.WithOrigins(
                "http://localhost:7071",
                "https://localhost:7071"
                
            )
         .AllowAnyHeader()
         .AllowAnyMethod());
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // auto apply migrations
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("blazor");

app.MapControllers();

app.Run();
