using Fauno.Agenda.Infrastructure.Http;
using Fauno.Register.Application.Interfaces.Http;
using Fauno.Register.Application.UseCases;
using Fauno.Register.Domain.Repositories;
using Fauno.Register.Infrastructure.Data.Context;
using Fauno.Register.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Configure ConnectionStrings:DefaultConnection in appsettings.json or environment variables.");

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var jwtSecret = jwtSettings["Secret"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
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

var serverVersion = new MySqlServerVersion(new Version(8, 4, 9));

builder.Services.AddDbContext<Context>(options =>
    options.UseMySql(connectionString, serverVersion));

builder.Services.AddScoped<IDonoRepository, DonoRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IVeterinarioRepository, VeterinarioRepository>();

builder.Services.AddScoped<CadastrarDonoUseCase>();
builder.Services.AddScoped<ObterDonoIdPorUserIdUseCase>();
builder.Services.AddScoped<VerificarDonoExisteUseCase>();
builder.Services.AddScoped<ObterVeterinarioIdPorUserIdUseCase>();
builder.Services.AddScoped<VerificarVeterinarioExisteUseCase>();
builder.Services.AddScoped<VerificarPetExisteUseCase>();
builder.Services.AddScoped<CadastrarPetUseCase>();
builder.Services.AddScoped<ListarPetsDoDonoUseCase>();
builder.Services.AddScoped<AtualizarPetUseCase>();
builder.Services.AddScoped<BuscarHistoricoPetUseCase>();
builder.Services.AddScoped<CadastrarVeterinarioUseCase>();

builder.Services.AddHttpClient<IAuthGateway, RegisterApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseUrl"]);
});

builder.Services.AddControllers();

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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();