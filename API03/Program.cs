using System.Text.Json.Serialization;
using API03.Infra;
using API03.Models;
using API03.Repositories;
using API03.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("EmpresaDb"));
 
builder.Services.AddScoped<ISetorRepository, SetorRepository>();
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();
 
builder.Services.AddScoped<ISetorService, SetorService>();
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
builder.Services.AddScoped<IEnderecoService, EnderecoService>();
 

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
var app = builder.Build();
 
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(context);
}
 
app.UseSwagger();
app.UseSwaggerUI();
 
app.MapControllers();
 
app.Run();