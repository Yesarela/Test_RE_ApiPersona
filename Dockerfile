FROM mcr.microsoft.com / dotnet / aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PersonaApi.csproj", "."]
RUN dotnet restore "./PersonaApi/PersonaApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "PersonaApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PersonaApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PersonaApi.dll"]
