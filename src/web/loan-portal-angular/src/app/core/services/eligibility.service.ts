import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { EligibilityResult } from '../models/eligibility.model';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class EligibilityService {
  private apiUrl = `${environment.eligibilityApiBaseUrl}/api/v1/eligibility`;

  constructor(private http: HttpClient) { }

  checkEligibility(applicationId: string): Observable<EligibilityResult> {
    return this.http.post<ApiResponse<EligibilityResult>>(`${this.apiUrl}/check`, { applicationId }).pipe(map(response => response.data));
  }

  getResultByApplicationId(applicationId: string): Observable<EligibilityResult> {
    return this.http.get<ApiResponse<EligibilityResult>>(`${this.apiUrl}/applications/${applicationId}`).pipe(map(response => response.data));
  }

  getResultById(id: string): Observable<EligibilityResult> {
    return this.http.get<ApiResponse<EligibilityResult>>(`${this.apiUrl}/results/${id}`).pipe(map(response => response.data));
  }
}
