import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { LoanApplicationService } from '../../../core/services/loan-application.service';
import { AuditService } from '../../../core/services/audit.service';
import { LoanApplication } from '../../../core/models/loan-application.model';
import { AuditEvent } from '../../../core/models/audit.model';

@Component({
  selector: 'app-loan-application-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './loan-application-detail.component.html',
  styleUrls: ['./loan-application-detail.component.css']
})
export class LoanApplicationDetailComponent implements OnInit {
  application: LoanApplication | null = null;
  auditEvents: AuditEvent[] = [];
  
  isLoading = true;
  isLoadingAudit = true;
  isUpdating = false;
  errorMessage = '';
  auditErrorMessage = '';

  ApplicationStatus: { [key: number]: string } = {
    1: 'Draft',
    2: 'Submitted',
    3: 'UnderReview',
    4: 'EligibilityPassed',
    5: 'EligibilityFailed',
    6: 'Approved',
    7: 'Rejected',
    8: 'Cancelled'
  };

  validNextStatuses: { value: number, label: string }[] = [];
  selectedNextStatus: string = '';
  transitionReason: string = '';

  constructor(
    private route: ActivatedRoute,
    private loanService: LoanApplicationService,
    private auditService: AuditService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadApplicationDetails(id);
    }
  }

  loadApplicationDetails(id: string): void {
    this.isLoading = true;
    this.isLoadingAudit = true;
    this.errorMessage = '';
    this.auditErrorMessage = '';

    this.loanService.getApplication(id).subscribe({
      next: (data) => {
        this.application = data;
        this.determineValidNextStatuses(data.status);
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err.message;
        this.isLoading = false;
      }
    });

    this.auditService.getEventsByEntity('LoanApplication', id).subscribe({
      next: (response) => {
        this.auditEvents = response.data;
        this.isLoadingAudit = false;
      },
      error: (err) => {
        this.auditErrorMessage = err.message;
        this.isLoadingAudit = false;
      }
    });
  }

  determineValidNextStatuses(currentStatusNum: number): void {
    this.validNextStatuses = [];
    
    // Reverse map ApplicationStatus
    let allowed: number[] = [];
    
    if (currentStatusNum === 2) allowed = [3, 8]; // Submitted -> UnderReview, Cancelled
    else if (currentStatusNum === 3) allowed = [4, 5]; // UnderReview -> EligibilityPassed, EligibilityFailed
    else if (currentStatusNum === 4) allowed = [6, 7]; // EligibilityPassed -> Approved, Rejected
    else if (currentStatusNum === 5) allowed = [7, 8]; // EligibilityFailed -> Rejected, Cancelled

    this.validNextStatuses = allowed.map(val => ({
      value: val,
      label: this.ApplicationStatus[val]
    }));

    if (this.validNextStatuses.length > 0) {
      this.selectedNextStatus = this.ApplicationStatus[this.validNextStatuses[0].value];
    }
  }

  updateStatus(): void {
    if (!this.application || !this.selectedNextStatus || !this.transitionReason) return;

    this.isUpdating = true;
    this.errorMessage = '';

    const request = {
      newStatus: this.selectedNextStatus,
      reason: this.transitionReason,
      changedBy: 'System Admin (Demo)'
    };

    this.loanService.updateApplicationStatus(this.application.id, request).subscribe({
      next: (app) => {
        this.application = app;
        this.determineValidNextStatuses(app.status);
        this.transitionReason = '';
        this.isUpdating = false;
        // Refresh audit after update
        this.loadApplicationDetails(app.id);
      },
      error: (err) => {
        this.errorMessage = err.message;
        this.isUpdating = false;
      }
    });
  }

  getStatusClass(statusNum: number): string {
    const status = this.ApplicationStatus[statusNum];
    switch (status) {
      case 'Submitted': return 'badge-info';
      case 'UnderReview': return 'badge-warning';
      case 'EligibilityPassed': return 'badge-success';
      case 'EligibilityFailed': return 'badge-error';
      case 'Approved': return 'badge-success';
      case 'Rejected': return 'badge-error';
      case 'Cancelled': return 'badge-secondary';
      default: return 'badge-secondary';
    }
  }

  getHistoryStatusClass(statusName: string): string {
    switch (statusName) {
      case 'Submitted': return 'badge-info';
      case 'UnderReview': return 'badge-warning';
      case 'EligibilityPassed': return 'badge-success';
      case 'EligibilityFailed': return 'badge-error';
      case 'Approved': return 'badge-success';
      case 'Rejected': return 'badge-error';
      case 'Cancelled': return 'badge-secondary';
      default: return 'badge-secondary';
    }
  }

  getLoanTypeName(type: number): string {
    const types: { [key: number]: string } = {
      1: 'Personal Loan',
      2: 'Home Loan',
      3: 'Auto Loan',
      4: 'Education Loan'
    };
    return types[type] || 'Unknown';
  }
}
