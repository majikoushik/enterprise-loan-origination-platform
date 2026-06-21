import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { CustomerRegistrationRequest, CustomerResponse } from '../models/customer.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  // Hardcoded for demo purposes. In a real app, use environment.apiUrl
  private apiUrl = 'http://localhost:5000/api/v1/customers';

  constructor(private http: HttpClient) { }

  registerCustomer(request: CustomerRegistrationRequest): Observable<CustomerResponse> {
    return this.http.post<CustomerResponse>(this.apiUrl, request).pipe(
      catchError(this.handleError)
    );
  }

  getCustomers(): Observable<CustomerResponse[]> {
    return this.http.get<CustomerResponse[]>(this.apiUrl).pipe(
      catchError(this.handleError)
    );
  }

  getCustomer(id: string): Observable<CustomerResponse> {
    return this.http.get<CustomerResponse>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred!';
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      if (error.error && error.error.detail) {
        errorMessage = error.error.detail;
      } else {
        errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
      }
    }
    return throwError(() => new Error(errorMessage));
  }
}
