param location string = resourceGroup().location
param appName string
param environment string
@secure()
param sqlAdministratorLoginPassword string
param sqlAdministratorLogin string = 'sqladmin'

var sqlServerName = '${appName}-sql-${environment}'

resource sqlServer 'Microsoft.Sql/servers@2023-05-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: sqlAdministratorLogin
    administratorLoginPassword: sqlAdministratorLoginPassword
  }
}

var databases = [
  'CustomerDb'
  'LoanApplicationDb'
  'EligibilityDb'
  'NotificationDb'
  'AuditDb'
]

resource sqlDatabases 'Microsoft.Sql/servers/databases@2023-05-01-preview' = [for db in databases: {
  parent: sqlServer
  name: '${appName}-${db}-${environment}'
  location: location
  sku: {
    name: 'Basic'
  }
}]

output sqlServerName string = sqlServer.name
output sqlServerFqdn string = sqlServer.properties.fullyQualifiedDomainName
