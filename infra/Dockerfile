FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Presentation/NightTasker.UserHub.Presentation.WebApi/NightTasker.UserHub.Presentation.WebApi.csproj", "Presentation/NightTasker.UserHub.Presentation.WebApi/"]
COPY ["Infrastructure/NightTasker.UserHub.Infrastructure.Persistence/NightTasker.UserHub.Infrastructure.Persistence.csproj", "Infrastructure/NightTasker.UserHub.Infrastructure.Persistence/"]
COPY ["Core/NightTasker.UserHub.Core.Application/NightTasker.UserHub.Core.Application.csproj", "Core/NightTasker.UserHub.Core.Application/"]
COPY ["Core/NightTasker.UserHub.Core.Domain/NightTasker.UserHub.Core.Domain.csproj", "Core/NightTasker.UserHub.Core.Domain/"]
RUN dotnet restore "Presentation/NightTasker.UserHub.Presentation.WebApi/NightTasker.UserHub.Presentation.WebApi.csproj"
COPY . .
WORKDIR "/src/Presentation/NightTasker.UserHub.Presentation.WebApi"
RUN dotnet build "NightTasker.UserHub.Presentation.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NightTasker.UserHub.Presentation.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NightTasker.UserHub.Presentation.WebApi.dll"]
