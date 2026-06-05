using Fauno.Register.Application.UseCases;
using Fauno.Register.Domain.Repositories;
using Fauno.Register.Infrastructure.Data.Context;
using Fauno.Register.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Context>(options =>
    options.UseInMemoryDatabase("FaunoRegisterDb"));

builder.Services.AddScoped<IDonoRepository, DonoRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IVeterinarioRepository, VeterinarioRepository>();
builder.Services.AddScoped<CadastrarDonoUseCase>();
builder.Services.AddScoped<CadastrarPetUseCase>();
builder.Services.AddScoped<ListarPetsDoDonoUseCase>();
builder.Services.AddScoped<AtualizarPetUseCase>();
builder.Services.AddScoped<BuscarHistoricoPetUseCase>();
builder.Services.AddScoped<CadastrarVeterinarioUseCase>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();