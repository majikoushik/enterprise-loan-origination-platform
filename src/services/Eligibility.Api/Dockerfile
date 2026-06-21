FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["EnterpriseLoanOriginationPlatform.sln", "./"]
COPY ["src/services/Eligibility.Api/Eligibility.Api.csproj", "src/services/Eligibility.Api/"]
COPY ["src/building-blocks/SharedKernel/SharedKernel.csproj", "src/building-blocks/SharedKernel/"]
COPY ["src/building-blocks/Observability/Observability.csproj", "src/building-blocks/Observability/"]
COPY ["src/building-blocks/Auditing/Auditing.csproj", "src/building-blocks/Auditing/"]
COPY ["src/building-blocks/Messaging/Messaging.csproj", "src/building-blocks/Messaging/"]

RUN dotnet restore "src/services/Eligibility.Api/Eligibility.Api.csproj"

COPY src/services/Eligibility.Api/ src/services/Eligibility.Api/
COPY src/building-blocks/ src/building-blocks/

WORKDIR /src/src/services/Eligibility.Api
RUN dotnet publish "Eligibility.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Eligibility.Api.dll"]
