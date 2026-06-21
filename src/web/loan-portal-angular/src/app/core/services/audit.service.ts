import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuditApiResponse } from '../models/audit.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuditService {
  private apiUrl = `${environment.auditApiBaseUrl}/api/v1/audit`;

  constructor(private http: HttpClient) { }

  getEvents(): Observable<AuditApiResponse> {
    return this.http.get<AuditApiResponse>(`${this.apiUrl}/events`);
  }

  getEventsByEntity(entityType: string, entityId: string): Observable<AuditApiResponse> {
    return this.http.get<AuditApiResponse>(`${this.apiUrl}/entity/${entityType}/${entityId}`);
  }
}
