import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { CustomerRegistrationRequest, CustomerResponse } from '../models/customer.model';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private apiUrl = `${environment.customerApiBaseUrl}/api/v1/customers`;

  constructor(private http: HttpClient) { }

  registerCustomer(request: CustomerRegistrationRequest): Observable<CustomerResponse> {
    return this.http.post<ApiResponse<CustomerResponse>>(this.apiUrl, request).pipe(map(response => response.data));
  }

  getCustomers(): Observable<CustomerResponse[]> {
    return this.http.get<ApiResponse<CustomerResponse[]>>(this.apiUrl).pipe(map(response => response.data));
  }

  getCustomer(id: string): Observable<CustomerResponse> {
    return this.http.get<ApiResponse<CustomerResponse>>(`${this.apiUrl}/${id}`).pipe(map(response => response.data));
  }
}
