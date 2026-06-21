import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CorrelationIdService {
  private readonly sessionId = crypto.randomUUID();

  getSessionId(): string {
    return this.sessionId;
  }

  newOperationId(): string {
    return crypto.randomUUID();
  }
}
