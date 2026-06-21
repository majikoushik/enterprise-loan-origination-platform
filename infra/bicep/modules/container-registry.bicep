param location string = resourceGroup().location
param appName string
param environment string

// ACR name must be alphanumeric only and globally unique
var registryName = replace('${appName}acr${environment}', '-', '')

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2023-01-01-preview' = {
  name: registryName
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
  }
}

output loginServer string = containerRegistry.properties.loginServer
output registryName string = containerRegistry.name
