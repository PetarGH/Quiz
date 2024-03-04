#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Unit_Tests/Unit_Tests.csproj", "Unit_Tests/"]
COPY ["Backend/Backend.csproj", "Backend/"]

RUN dotnet restore "Domain/Domain.csproj"
RUN dotnet restore "Application/Application.csproj"
RUN dotnet restore "Infrastructure/Infrastructure.csproj"
RUN dotnet restore "Unit_Tests/Unit_Tests.csproj"
RUN dotnet restore "Backend/Backend.csproj"

COPY . .

WORKDIR "/src/Backend"
RUN dotnet build "Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.dll"]
