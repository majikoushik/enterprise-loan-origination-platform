FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files for dependency restore
COPY ["EnterpriseLoanOriginationPlatform.sln", "./"]
COPY ["src/services/LoanApplication.Api/LoanApplication.Api.csproj", "src/services/LoanApplication.Api/"]
COPY ["src/building-blocks/SharedKernel/SharedKernel.csproj", "src/building-blocks/SharedKernel/"]
COPY ["src/building-blocks/Observability/Observability.csproj", "src/building-blocks/Observability/"]
COPY ["src/building-blocks/Auditing/Auditing.csproj", "src/building-blocks/Auditing/"]
COPY ["src/building-blocks/Messaging/Messaging.csproj", "src/building-blocks/Messaging/"]

RUN dotnet restore "src/services/LoanApplication.Api/LoanApplication.Api.csproj"

# Copy the rest of the source code
COPY src/services/LoanApplication.Api/ src/services/LoanApplication.Api/
COPY src/building-blocks/ src/building-blocks/

WORKDIR /src/src/services/LoanApplication.Api
RUN dotnet publish "LoanApplication.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "LoanApplication.Api.dll"]
