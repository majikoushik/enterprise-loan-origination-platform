FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["EnterpriseLoanOriginationPlatform.sln", "./"]
COPY ["src/services/Audit.Api/Audit.Api.csproj", "src/services/Audit.Api/"]
COPY ["src/building-blocks/SharedKernel/SharedKernel.csproj", "src/building-blocks/SharedKernel/"]
COPY ["src/building-blocks/Observability/Observability.csproj", "src/building-blocks/Observability/"]

RUN dotnet restore "src/services/Audit.Api/Audit.Api.csproj"

COPY src/services/Audit.Api/ src/services/Audit.Api/
COPY src/building-blocks/ src/building-blocks/

WORKDIR /src/src/services/Audit.Api
RUN dotnet publish "Audit.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Audit.Api.dll"]
