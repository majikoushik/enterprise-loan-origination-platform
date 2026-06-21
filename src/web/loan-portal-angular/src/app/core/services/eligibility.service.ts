import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EligibilityResult } from '../models/eligibility.model';

@Injectable({
  providedIn: 'root'
})
export class EligibilityService {
  private apiUrl = 'http://localhost:5002/api/v1/eligibility';

  constructor(private http: HttpClient) { }

  checkEligibility(applicationId: string): Observable<EligibilityResult> {
    return this.http.post<EligibilityResult>(`${this.apiUrl}/check`, { applicationId }).pipe(
      catchError(this.handleError)
    );
  }

  getResultByApplicationId(applicationId: string): Observable<EligibilityResult> {
    return this.http.get<EligibilityResult>(`${this.apiUrl}/applications/${applicationId}`).pipe(
      catchError(this.handleError)
    );
  }

  getResultById(id: string): Observable<EligibilityResult> {
    return this.http.get<EligibilityResult>(`${this.apiUrl}/results/${id}`).pipe(
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
