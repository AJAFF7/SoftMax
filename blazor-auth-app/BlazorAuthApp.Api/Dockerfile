FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY BlazorAuthApp.Api/BlazorAuthApp.Api.csproj BlazorAuthApp.Api/
RUN dotnet restore "BlazorAuthApp.Api/BlazorAuthApp.Api.csproj"

# Copy everything else and build
COPY BlazorAuthApp.Api/ BlazorAuthApp.Api/
WORKDIR "/src/BlazorAuthApp.Api"
RUN dotnet build "BlazorAuthApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorAuthApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Create migration bundle in build stage
FROM build AS efbundle
WORKDIR "/src/BlazorAuthApp.Api"
RUN dotnet tool install --global dotnet-ef --version 10.*-*
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet ef migrations bundle --runtime linux-arm64 -o /app/efbundle || \
    dotnet ef migrations bundle -o /app/efbundle

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app

COPY --from=publish /app/publish .
COPY --from=efbundle /app/efbundle /app/efbundle

# Copy entrypoint script
COPY BlazorAuthApp.Api/docker-entrypoint.sh /app/docker-entrypoint.sh
RUN chmod +x /app/docker-entrypoint.sh /app/efbundle

EXPOSE 8080

ENTRYPOINT ["/app/docker-entrypoint.sh"]
