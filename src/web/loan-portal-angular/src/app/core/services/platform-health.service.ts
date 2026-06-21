import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import { ServiceMetadata } from '../models/service-health.model';

@Injectable({ providedIn: 'root' })
export class PlatformHealthService {
  constructor(private readonly http: HttpClient) {}

  getCustomerServiceMetadata(): Observable<ApiResponse<ServiceMetadata>> {
    return this.http.get<ApiResponse<ServiceMetadata>>(
      `${environment.customerApiBaseUrl}/api/v1/customer-service/metadata`
    );
  }
}
