import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { NotificationRequestResponse } from '../models/notification.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private apiUrl = `${environment.notificationApiBaseUrl}/api/v1/notifications`;

  constructor(private http: HttpClient) { }

  getNotifications(): Observable<NotificationRequestResponse[]> {
    return this.http.get<NotificationRequestResponse[]>(this.apiUrl);
  }
}
