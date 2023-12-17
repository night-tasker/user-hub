FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NightTasker.UserHub.Presentation.WebApi/NightTasker.UserHub.Presentation.WebApi.csproj", "NightTasker.UserHub.Presentation.WebApi/"]
RUN dotnet restore "NightTasker.UserHub.Presentation.WebApi/NightTasker.UserHub.Presentation.WebApi.csproj"
COPY . .
WORKDIR "/src/NightTasker.UserHub.Presentation.WebApi"
RUN dotnet build "NightTasker.UserHub.Presentation.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NightTasker.UserHub.Presentation.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NightTasker.UserHub.Presentation.WebApi.dll"]
