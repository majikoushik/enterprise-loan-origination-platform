param location string = resourceGroup().location
param appName string
param environment string

var serviceBusName = '${appName}-sb-${environment}'

resource serviceBusNamespace 'Microsoft.ServiceBus/namespaces@2022-10-01-preview' = {
  name: serviceBusName
  location: location
  sku: {
    name: 'Standard'
    tier: 'Standard'
  }
}

// Topics for MVP (or placeholder for future queue setups)
resource notificationTopic 'Microsoft.ServiceBus/namespaces/topics@2022-10-01-preview' = {
  parent: serviceBusNamespace
  name: 'notification-requests'
}

resource auditTopic 'Microsoft.ServiceBus/namespaces/topics@2022-10-01-preview' = {
  parent: serviceBusNamespace
  name: 'audit-events'
}

output serviceBusName string = serviceBusNamespace.name
output serviceBusEndpoint string = serviceBusNamespace.properties.serviceBusEndpoint
