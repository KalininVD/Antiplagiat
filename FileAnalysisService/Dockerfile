FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE ${API_PORT}

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FileAnalysisService/FileAnalysisService.Web/FileAnalysisService.Web.csproj", "FileAnalysisService.Web/"]
COPY ["FileAnalysisService/FileAnalysisService.Application/FileAnalysisService.Application.csproj", "FileAnalysisService.Application/"]
COPY ["FileAnalysisService/FileAnalysisService.Domain/FileAnalysisService.Domain.csproj", "FileAnalysisService.Domain/"]
COPY ["FileAnalysisService/FileAnalysisService.Infrastructure/FileAnalysisService.Infrastructure.csproj", "FileAnalysisService.Infrastructure/"]
RUN dotnet restore "FileAnalysisService.Web/FileAnalysisService.Web.csproj"
COPY FileAnalysisService/FileAnalysisService.Web/ ./FileAnalysisService.Web/
COPY FileAnalysisService/FileAnalysisService.Application/ ./FileAnalysisService.Application/
COPY FileAnalysisService/FileAnalysisService.Domain/ ./FileAnalysisService.Domain/
COPY FileAnalysisService/FileAnalysisService.Infrastructure/ ./FileAnalysisService.Infrastructure/
WORKDIR "/src/FileAnalysisService.Web"
RUN dotnet build "FileAnalysisService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FileAnalysisService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileAnalysisService.Web.dll"]