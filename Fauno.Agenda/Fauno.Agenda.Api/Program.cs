using Fauno.Agenda.Application.Interfaces.Http;
using Fauno.Agenda.Infrastructure.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddHttpClient<IRegisterGateway, RegisterApiClient>(
    client =>
    {
        client.BaseAddress = new Uri("http://localhost:8000/");
    });

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
