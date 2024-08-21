FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "QuartzJobs/QuartzJobs.csproj"
WORKDIR "/src/Quartz"
RUN dotnet build "/src/QuartzJobs/QuartzJobs.csproj" -c Release --no-restore -o /app/build

FROM build AS publish
RUN dotnet publish "/src/QuartzJobs/QuartzJobs.csproj" -c Release --no-restore -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuartzJobs.dll"]