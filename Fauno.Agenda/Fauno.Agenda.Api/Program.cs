using Fauno.Agenda.Application.Interfaces.Http;
using Fauno.Agenda.Application.UseCases;
using Fauno.Agenda.Domain.Interfaces.Repositories;
using Fauno.Agenda.Infrastructure.Http;
using Fauno.Agenda.Infrastructure.Persistence;
using Fauno.Agenda.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddDbContext<AgendaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAvailabilityRuleRepository, AvailabilityRuleRepository>();
builder.Services.AddScoped<IAvailabilityExceptionRepository, AvailabilityExceptionRepository>();

// Use Cases
builder.Services.AddScoped<MakeAppointmentUseCase>();
builder.Services.AddScoped<CancelAppointmentUseCase>();
builder.Services.AddScoped<ConfirmAppointmentUseCase>();
builder.Services.AddScoped<FinishAppointmentUseCase>();
builder.Services.AddScoped<GetAppointmentsByVeterinarianUseCase>();
builder.Services.AddScoped<GetAppointmentsByOwnerUseCase>();
builder.Services.AddScoped<CreateAvailabilityRuleUseCase>();
builder.Services.AddScoped<RemoveAvailabilityRuleUseCase>();
builder.Services.AddScoped<CreateAvailabilityExceptionUseCase>();
builder.Services.AddScoped<GetAvailableSlotsUseCase>();


builder.Services.AddHttpClient<IRegisterGateway, RegisterApiClient>(
    client =>
    {
        client.BaseAddress = new Uri("http://localhost:8000/");
    });

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment() ||
    app.Environment.IsEnvironment("Docker"))
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AgendaDbContext>();

        Console.WriteLine(
            builder.Configuration.GetConnectionString("DefaultConnection"));

        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao aplicar migrations: {ex}");
        throw;
    }
}

app.Run();
