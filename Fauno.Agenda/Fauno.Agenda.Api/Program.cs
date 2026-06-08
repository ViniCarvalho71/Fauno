using Fauno.Agenda.Application.Interfaces.Http;
using Fauno.Agenda.Application.UseCases;
using Fauno.Agenda.Domain.Interfaces.Repositories;
using Fauno.Agenda.Infrastructure.Http;
using Fauno.Agenda.Infrastructure.Persistence;
using Fauno.Agenda.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var jwtSecret = jwtSettings["Secret"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret!))
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("JWT ERROR:");
                Console.WriteLine(context.Exception.ToString());
                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                Console.WriteLine("TOKEN VALIDADO");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));

builder.Services.AddDbContext<AgendaDbContext>(options =>
    options.UseMySql(connectionString, serverVersion));

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAvailabilityRuleRepository, AvailabilityRuleRepository>();
builder.Services.AddScoped<IAvailabilityExceptionRepository, AvailabilityExceptionRepository>();


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

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<JwtDelegatingHandler>();
builder.Services.AddHttpClient<IRegisterGateway, RegisterApiClient>(
    client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["RegisterApi:BaseUrl"]);

    })
    .AddHttpMessageHandler<JwtDelegatingHandler>();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();

        document.Components.SecuritySchemes ??=
            new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] =
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "Informe apenas o JWT"
            };

        return Task.CompletedTask;
    });
});



var app = builder.Build();

if (app.Environment.IsDevelopment() ||
    app.Environment.IsEnvironment("Docker"))
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => options
    .AddPreferredSecuritySchemes("Bearer")
    .EnablePersistentAuthentication());
}

app.UseHttpsRedirection();


app.UseAuthentication();
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
