FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files for dependency restore
COPY ["EnterpriseLoanOriginationPlatform.sln", "./"]
COPY ["src/services/Customer.Api/Customer.Api.csproj", "src/services/Customer.Api/"]
COPY ["src/building-blocks/SharedKernel/SharedKernel.csproj", "src/building-blocks/SharedKernel/"]
COPY ["src/building-blocks/Observability/Observability.csproj", "src/building-blocks/Observability/"]
COPY ["src/building-blocks/Auditing/Auditing.csproj", "src/building-blocks/Auditing/"]

# Just restore the single project (which restores its references)
RUN dotnet restore "src/services/Customer.Api/Customer.Api.csproj"

# Copy the rest of the source code
COPY src/services/Customer.Api/ src/services/Customer.Api/
COPY src/building-blocks/ src/building-blocks/

WORKDIR /src/src/services/Customer.Api
RUN dotnet publish "Customer.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose standard port
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Customer.Api.dll"]
