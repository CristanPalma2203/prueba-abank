using Microsoft.AspNetCore.Identity;
using PruebaTecnica.Data;
using PruebaTecnica.Models;
using PruebaTecnica.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => 
    options.AddPolicy("Angular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowCredentials()
            .AllowAnyHeader();
    })
 );
// Configurar Database Context
builder.Services.AddTransient<DatabaseContext>();
builder.Services.AddTransient<EmpleadoRepository>();
builder.Services.AddTransient<UsuarioRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

app.UseCors("Angular");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
