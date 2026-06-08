FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Fauno.Register.Api/Fauno.Register.Api.csproj", "Fauno.Register.Api/"]
COPY ["Fauno.Register.Application/Fauno.Register.Application.csproj", "Fauno.Register.Application/"]
COPY ["Fauno.Register.Domain/Fauno.Register.Domain.csproj", "Fauno.Register.Domain/"]
COPY ["Fauno.Register.Infrastructure/Fauno.Register.Infrastructure.csproj", "Fauno.Register.Infrastructure/"]
RUN dotnet restore "Fauno.Register.Api/Fauno.Register.Api.csproj"

COPY . .
WORKDIR /src/Fauno.Register.Api
RUN dotnet publish "Fauno.Register.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Fauno.Register.Api.dll"]
