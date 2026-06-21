import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuditApiResponse } from '../models/audit.model';

@Injectable({
  providedIn: 'root'
})
export class AuditService {
  private apiUrl = 'http://localhost:5005/api/v1/audit';

  constructor(private http: HttpClient) { }

  getEvents(): Observable<AuditApiResponse> {
    return this.http.get<AuditApiResponse>(`${this.apiUrl}/events`).pipe(
      catchError(this.handleError)
    );
  }

  getEventsByEntity(entityType: string, entityId: string): Observable<AuditApiResponse> {
    return this.http.get<AuditApiResponse>(`${this.apiUrl}/entity/${entityType}/${entityId}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred!';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      if (error.error && error.error.detail) {
        errorMessage = error.error.detail;
      } else {
        errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
      }
    }
    return throwError(() => new Error(errorMessage));
  }
}
