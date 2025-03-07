using Microsoft.EntityFrameworkCore;
using VendasAPI.Data;
using VendasAPI.Models;
using VendasAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar o banco em mem�ria
builder.Services.AddDbContext<VendasContext>(options =>
    options.UseInMemoryDatabase("VendasDB"));

// Registrar servi�os
builder.Services.AddScoped<VendaService>();
builder.Services.AddHttpClient<FaturamentoService>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader(); 
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<VendasContext>();
    context.Clientes.Add(new Cliente { ClienteId = Guid.NewGuid(), Nome = "Cliente Teste", Categoria = "VIP" });
    context.SaveChanges();
}

app.Run();