import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoanApplicationService } from '../../../core/services/loan-application.service';
import { LoanApplicationResponse, ApplicationStatus, LoanType } from '../../../core/models/loan-application.model';

@Component({
  selector: 'app-loan-application-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './loan-application-list.component.html',
  styleUrls: ['./loan-application-list.component.css']
})
export class LoanApplicationListComponent implements OnInit {
  applications: LoanApplicationResponse[] = [];
  isLoading = true;
  errorMessage: string | null = null;
  
  ApplicationStatus = ApplicationStatus;
  LoanType = LoanType;

  constructor(private loanService: LoanApplicationService) {}

  ngOnInit(): void {
    this.loadApplications();
  }

  loadApplications(): void {
    this.isLoading = true;
    this.loanService.getApplications().subscribe({
      next: (data) => {
        this.applications = data;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load applications. Ensure the API is running.';
        this.isLoading = false;
      }
    });
  }

  getStatusClass(status: ApplicationStatus): string {
    switch (status) {
      case ApplicationStatus.Submitted: return 'badge-submitted';
      case ApplicationStatus.UnderReview: return 'badge-review';
      case ApplicationStatus.Approved: return 'badge-approved';
      case ApplicationStatus.Rejected: return 'badge-rejected';
      case ApplicationStatus.EligibilityPassed: return 'badge-passed';
      case ApplicationStatus.EligibilityFailed: return 'badge-failed';
      default: return 'badge-default';
    }
  }

  getLoanTypeName(type: LoanType): string {
    switch (type) {
      case LoanType.PersonalLoan: return 'Personal Loan';
      case LoanType.HomeLoan: return 'Home Loan';
      case LoanType.VehicleLoan: return 'Vehicle Loan';
      case LoanType.EducationLoan: return 'Education Loan';
      default: return 'Unknown';
    }
  }
}
