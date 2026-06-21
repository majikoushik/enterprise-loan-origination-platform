param location string = resourceGroup().location
param containerAppName string
param environmentId string
param containerImage string
param targetPort int = 8080
param isExternalIngress bool = false
param environmentVariables array = []
@secure()
param secrets array = []
param registryLoginServer string
@secure()
param registryPassword string
param registryUsername string

resource containerApp 'Microsoft.App/containerApps@2023-05-01' = {
  name: containerAppName
  location: location
  properties: {
    managedEnvironmentId: environmentId
    configuration: {
      ingress: {
        external: isExternalIngress
        targetPort: targetPort
      }
      registries: [
        {
          server: registryLoginServer
          username: registryUsername
          passwordSecretRef: 'registry-password'
        }
      ]
      secrets: union([
        {
          name: 'registry-password'
          value: registryPassword
        }
      ], secrets)
    }
    template: {
      containers: [
        {
          name: containerAppName
          image: containerImage
          env: environmentVariables
          probes: [
            {
              type: 'Liveness'
              httpGet: {
                port: targetPort
                path: '/health/live'
              }
              initialDelaySeconds: 15
              periodSeconds: 10
            }
            {
              type: 'Readiness'
              httpGet: {
                port: targetPort
                path: '/health/ready'
              }
              initialDelaySeconds: 15
              periodSeconds: 10
            }
          ]
        }
      ]
      scale: {
        minReplicas: 0
        maxReplicas: 5
      }
    }
  }
}

output fqdn string = containerApp.properties.configuration.ingress.fqdn
