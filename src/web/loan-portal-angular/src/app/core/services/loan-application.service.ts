import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoanApplicationRequest, LoanApplicationResponse, ApplicationStatusHistoryResponse, UpdateApplicationStatusRequest } from '../models/loan-application.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class LoanApplicationService {
  private apiUrl = `${environment.loanApplicationApiBaseUrl}/api/v1/loan-applications`;

  constructor(private http: HttpClient) { }

  submitApplication(request: LoanApplicationRequest): Observable<LoanApplicationResponse> {
    return this.http.post<ApiResponse<LoanApplicationResponse>>(this.apiUrl, request).pipe(map(response => response.data));
  }

  getApplications(): Observable<LoanApplicationResponse[]> {
    return this.http.get<ApiResponse<LoanApplicationResponse[]>>(this.apiUrl).pipe(map(response => response.data));
  }

  getApplication(id: string): Observable<LoanApplicationResponse> {
    return this.http.get<ApiResponse<LoanApplicationResponse>>(`${this.apiUrl}/${id}`).pipe(map(response => response.data));
  }

  getApplicationsByCustomer(customerId: string): Observable<LoanApplicationResponse[]> {
    return this.http.get<ApiResponse<LoanApplicationResponse[]>>(`${this.apiUrl}/customer/${customerId}`).pipe(map(response => response.data));
  }

  getApplicationStatusHistory(id: string): Observable<ApplicationStatusHistoryResponse[]> {
    return this.http.get<ApiResponse<ApplicationStatusHistoryResponse[]>>(`${this.apiUrl}/${id}/status-history`).pipe(map(response => response.data));
  }

  updateApplicationStatus(id: string, request: UpdateApplicationStatusRequest): Observable<LoanApplicationResponse> {
    return this.http.patch<ApiResponse<LoanApplicationResponse>>(`${this.apiUrl}/${id}/status`, request).pipe(map(response => response.data));
  }
}
