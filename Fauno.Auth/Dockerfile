FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Fauno.Auth.Domain/Fauno.Auth.Domain.csproj", "Fauno.Auth.Domain/"]
COPY ["Fauno.Auth.Application/Fauno.Auth.Application.csproj", "Fauno.Auth.Application/"]
COPY ["Fauno.Auth.Repository/Fauno.Auth.Infrastructure.csproj", "Fauno.Auth.Repository/"]
COPY ["Fauno.Auth.Api/Fauno.Auth.Api.csproj", "Fauno.Auth.Api/"]

RUN dotnet restore "Fauno.Auth.Api/Fauno.Auth.Api.csproj"

COPY . .
WORKDIR /src/Fauno.Auth.Api
RUN dotnet publish "Fauno.Auth.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Fauno.Auth.Api.dll"]