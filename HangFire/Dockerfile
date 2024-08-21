FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "HangFire/HangFire.csproj"
WORKDIR "/src/HangFire"
RUN dotnet build "/src/HangFire/HangFire.csproj" -c Release --no-restore -o /app/build

FROM build AS publish
RUN dotnet publish "/src/HangFire/HangFire.csproj" -c Release --no-restore -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HangFire.dll"]