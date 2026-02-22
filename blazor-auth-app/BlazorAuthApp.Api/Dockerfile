FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy NuGet config with retry settings
COPY BlazorAuthApp.Api/NuGet.Config ./

# Copy csproj and restore dependencies with retry logic
COPY BlazorAuthApp.Api/BlazorAuthApp.Api.csproj BlazorAuthApp.Api/
RUN for i in 1 2 3 4 5; do \
      dotnet restore "BlazorAuthApp.Api/BlazorAuthApp.Api.csproj" --configfile NuGet.Config && break || \
      { echo "Restore attempt $i failed, retrying..."; sleep 10; }; \
    done

# Copy everything else and build
COPY BlazorAuthApp.Api/ BlazorAuthApp.Api/
WORKDIR "/src/BlazorAuthApp.Api"
RUN dotnet build "BlazorAuthApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorAuthApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage (migrations will run at runtime via docker-entrypoint.sh)
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app

COPY --from=publish /app/publish .

# Copy entrypoint script
COPY BlazorAuthApp.Api/docker-entrypoint.sh /app/docker-entrypoint.sh
RUN chmod +x /app/docker-entrypoint.sh

EXPOSE 8080

ENTRYPOINT ["/app/docker-entrypoint.sh"]
