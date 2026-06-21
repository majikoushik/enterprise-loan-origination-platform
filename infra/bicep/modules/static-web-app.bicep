param location string = resourceGroup().location
param appName string
param environment string

var staticWebAppName = '${appName}-swa-${environment}'

resource staticWebApp 'Microsoft.Web/staticSites@2022-09-01' = {
  name: staticWebAppName
  location: location
  sku: {
    name: 'Free'
    tier: 'Free'
  }
  properties: {}
}

output staticWebAppDefaultHostName string = staticWebApp.properties.defaultHostname
