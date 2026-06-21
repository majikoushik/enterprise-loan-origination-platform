import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuditService } from '../../core/services/audit.service';
import { AuditEvent } from '../../core/models/audit.model';

@Component({
  selector: 'app-audit-trail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './audit-trail.component.html',
  styleUrls: ['./audit-trail.component.css']
})
export class AuditTrailComponent implements OnInit {
  events: AuditEvent[] = [];
  isLoading = true;
  errorMessage = '';
  expandedRowId: string | null = null;

  constructor(private auditService: AuditService) {}

  ngOnInit(): void {
    this.loadEvents();
  }

  loadEvents(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.auditService.getEvents().subscribe({
      next: (response) => {
        this.events = response.data;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err.message;
        this.isLoading = false;
      }
    });
  }

  toggleRow(id: string): void {
    if (this.expandedRowId === id) {
      this.expandedRowId = null;
    } else {
      this.expandedRowId = id;
    }
  }

  getSeverityClass(severity: string): string {
    switch (severity.toLowerCase()) {
      case 'info': return 'badge-info';
      case 'warning': return 'badge-warning';
      case 'error':
      case 'critical': return 'badge-error';
      default: return 'badge-secondary';
    }
  }

  getCategoryClass(category: string): string {
    switch (category.toLowerCase()) {
      case 'customer': return 'badge-primary';
      case 'loanapplication': return 'badge-success';
      case 'eligibility': return 'badge-secondary';
      case 'notification': return 'badge-warning';
      default: return 'badge-info';
    }
  }

  formatMetadata(json: string): string {
    try {
      const parsed = JSON.parse(json);
      return JSON.stringify(parsed, null, 2);
    } catch {
      return json;
    }
  }
}
