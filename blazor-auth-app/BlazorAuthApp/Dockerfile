FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY BlazorAuthApp/BlazorAuthApp.csproj BlazorAuthApp/
RUN dotnet restore "BlazorAuthApp/BlazorAuthApp.csproj"

# Copy everything else
COPY BlazorAuthApp/ BlazorAuthApp/
WORKDIR "/src/BlazorAuthApp"

# Build and publish
RUN dotnet build "BlazorAuthApp.csproj" -c Release -o /app/build
RUN dotnet publish "BlazorAuthApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage - use nginx to serve the static files
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Copy published Blazor files
COPY --from=build /app/publish/wwwroot .

# Copy custom nginx config
COPY BlazorAuthApp/nginx.conf /etc/nginx/nginx.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
