FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Fauno.Agenda.Api/Fauno.Agenda.Api.csproj              Fauno.Agenda.Api/
COPY Fauno.Agenda.Application/Fauno.Agenda.Application.csproj  Fauno.Agenda.Application/
COPY Fauno.Agenda.Domain/Fauno.Agenda.Domain.csproj        Fauno.Agenda.Domain/
COPY Fauno.Agenda.Infrastructure/Fauno.Agenda.Infrastructure.csproj Fauno.Agenda.Infrastructure/

RUN dotnet restore Fauno.Agenda.Api/Fauno.Agenda.Api.csproj

COPY . .
RUN dotnet publish Fauno.Agenda.Api/Fauno.Agenda.Api.csproj \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Docker

ENTRYPOINT ["dotnet", "Fauno.Agenda.Api.dll"]