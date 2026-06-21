param appName string = 'entloan'
param environment string = 'dev'
param location string = resourceGroup().location

@secure()
param sqlAdministratorLoginPassword string

module logAnalytics 'modules/log-analytics.bicep' = {
  name: 'logAnalyticsDeployment'
  params: {
    appName: appName
    environment: environment
    location: location
  }
}

module appInsights 'modules/application-insights.bicep' = {
  name: 'appInsightsDeployment'
  params: {
    appName: appName
    environment: environment
    location: location
    logAnalyticsWorkspaceId: logAnalytics.outputs.workspaceId
  }
}

module keyVault 'modules/key-vault.bicep' = {
  name: 'keyVaultDeployment'
  params: {
    appName: appName
    environment: environment
    location: location
  }
}

module serviceBus 'modules/service-bus.bicep' = {
  name: 'serviceBusDeployment'
  params: {
    appName: appName
    environment: environment
    location: location
  }
}

module sqlDatabase 'modules/sql-database.bicep' = {
  name: 'sqlDatabaseDeployment'
  params: {
    appName: appName
    environment: environment
    location: location
    sqlAdministratorLoginPassword: sqlAdministratorLoginPassword
  }
}

module containerRegistry 'modules/container-registry.bicep' = {
  name: 'containerRegistryDeployment'
  params: {
    appName: appName
    environment: environment
    location: location
  }
}

module containerAppsEnv 'modules/container-apps-environment.bicep' = {
  name: 'containerAppsEnvDeployment'
  params: {
    appName: appName
    environment: environment
    location: location
    logAnalyticsWorkspaceId: logAnalytics.outputs.workspaceId
  }
}

module staticWebApp 'modules/static-web-app.bicep' = {
  name: 'staticWebAppDeployment'
  params: {
    appName: appName
    environment: environment
    location: location
  }
}
