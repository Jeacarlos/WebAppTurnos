using Microsoft.EntityFrameworkCore;
using WebAppTurnos.Context;
using WebAppTurnos.Mapper;
using WebAppTurnos.Repositorios;
using WebAppTurnos.Repositorios.IRepositorio;


var builder = WebApplication.CreateBuilder(args);



//creacion de variable para la cadena de conexion con la base de datos
var connectionString = builder.Configuration.GetConnectionString("Connection");
//registrar servicios para la conexion
builder.Services.AddDbContext<AppDbContext_context>(options => options.UseSqlServer(connectionString));

//IMPLEMENTACION DE REPOSITORIO
builder.Services.AddTransient<ITurnoRepositorio, TurnoRepositorio>();
builder.Services.AddTransient<IDocumentoRepositorio, DocumentoRepositorio>();
builder.Services.AddTransient<IEmpleadoRepositorio, EmpleadoRepositorio>();

//Servicios de los AutoMapper
builder.Services.AddAutoMapper(typeof(TurnoMapper));

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
