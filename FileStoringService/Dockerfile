FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE ${API_PORT}

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FileStoringService/FileStoringService.Web/FileStoringService.Web.csproj", "FileStoringService.Web/"]
COPY ["FileStoringService/FileStoringService.Application/FileStoringService.Application.csproj", "FileStoringService.Application/"]
COPY ["FileStoringService/FileStoringService.Domain/FileStoringService.Domain.csproj", "FileStoringService.Domain/"]
COPY ["FileStoringService/FileStoringService.Infrastructure/FileStoringService.Infrastructure.csproj", "FileStoringService.Infrastructure/"]
RUN dotnet restore "FileStoringService.Web/FileStoringService.Web.csproj"
COPY FileStoringService/FileStoringService.Web/ ./FileStoringService.Web/
COPY FileStoringService/FileStoringService.Application/ ./FileStoringService.Application/
COPY FileStoringService/FileStoringService.Domain/ ./FileStoringService.Domain/
COPY FileStoringService/FileStoringService.Infrastructure/ ./FileStoringService.Infrastructure/
WORKDIR "/src/FileStoringService.Web"
RUN dotnet build "FileStoringService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FileStoringService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileStoringService.Web.dll"]