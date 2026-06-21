export interface NotificationRequestResponse {
  id: string;
  correlationId: string;
  eventType: string;
  entityType: string;
  entityId: string;
  customerId?: string;
  recipient: string;
  channel: string;
  subject: string;
  messageBody: string;
  status: string;
  createdAtUtc: string;
  processedAtUtc?: string;
  failureReason?: string;
  retryCount: number;
}
