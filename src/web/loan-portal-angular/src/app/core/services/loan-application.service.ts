import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { LoanApplicationRequest, LoanApplicationResponse } from '../models/loan-application.model';

@Injectable({
  providedIn: 'root'
})
export class LoanApplicationService {
  private apiUrl = 'http://localhost:5001/api/v1/loan-applications';

  constructor(private http: HttpClient) { }

  submitApplication(request: LoanApplicationRequest): Observable<LoanApplicationResponse> {
    return this.http.post<LoanApplicationResponse>(this.apiUrl, request).pipe(
      catchError(this.handleError)
    );
  }

  getApplications(): Observable<LoanApplicationResponse[]> {
    return this.http.get<LoanApplicationResponse[]>(this.apiUrl).pipe(
      catchError(this.handleError)
    );
  }

  getApplication(id: string): Observable<LoanApplicationResponse> {
    return this.http.get<LoanApplicationResponse>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  getApplicationsByCustomer(customerId: string): Observable<LoanApplicationResponse[]> {
    return this.http.get<LoanApplicationResponse[]>(`${this.apiUrl}/customer/${customerId}`).pipe(
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
