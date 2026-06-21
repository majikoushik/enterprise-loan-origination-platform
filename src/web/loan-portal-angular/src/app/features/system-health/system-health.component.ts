import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { forkJoin, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

interface ServiceHealth {
  name: string;
  url: string;
  status: 'Healthy' | 'Unhealthy' | 'Checking...';
}

@Component({
  selector: 'app-system-health',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './system-health.component.html',
  styleUrls: ['./system-health.component.css']
})
export class SystemHealthComponent implements OnInit {
  services: ServiceHealth[] = [
    { name: 'Customer API', url: 'http://localhost:7101/health/live', status: 'Checking...' },
    { name: 'Loan Application API', url: 'http://localhost:7102/health/live', status: 'Checking...' },
    { name: 'Eligibility API', url: 'http://localhost:7103/health/live', status: 'Checking...' },
    { name: 'Notification Worker API', url: 'http://localhost:5004/health/live', status: 'Checking...' },
    { name: 'Audit API', url: 'http://localhost:5005/health/live', status: 'Checking...' }
  ];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.checkHealth();
  }

  checkHealth(): void {
    this.services.forEach(s => s.status = 'Checking...');

    const requests = this.services.map(service => 
      this.http.get(service.url, { responseType: 'text' }).pipe(
        catchError(() => of('Unhealthy'))
      )
    );

    forkJoin(requests).subscribe(results => {
      results.forEach((result, index) => {
        this.services[index].status = result === 'Healthy' ? 'Healthy' : 'Unhealthy';
      });
    });
  }

  getStatusClass(status: string): string {
    if (status === 'Healthy') return 'badge-success';
    if (status === 'Unhealthy') return 'badge-error';
    return 'badge-secondary';
  }
}
