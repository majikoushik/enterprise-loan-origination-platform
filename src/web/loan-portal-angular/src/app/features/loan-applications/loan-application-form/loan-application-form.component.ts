import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { LoanApplicationService } from '../../../core/services/loan-application.service';
import { LoanType, ApplicationStatus } from '../../../core/models/loan-application.model';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-loan-application-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './loan-application-form.component.html',
  styleUrls: ['./loan-application-form.component.css']
})
export class LoanApplicationFormComponent implements OnInit {
  loanForm: FormGroup;
  isLoading = false;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  submittedApplicationId: string | null = null;
  submittedApplicationStatus: string | null = null;
  
  loanTypes = [
    { value: LoanType.PersonalLoan, label: 'Personal Loan' },
    { value: LoanType.HomeLoan, label: 'Home Loan' },
    { value: LoanType.VehicleLoan, label: 'Vehicle Loan' },
    { value: LoanType.EducationLoan, label: 'Education Loan' }
  ];

  constructor(
    private fb: FormBuilder,
    private loanService: LoanApplicationService,
    private router: Router
  ) {
    this.loanForm = this.fb.group({
      customerId: ['', Validators.required],
      loanType: ['', Validators.required],
      requestedAmount: [null, [Validators.required, Validators.min(1)]],
      requestedTenureInMonths: [null, [Validators.required, Validators.min(6), Validators.max(84)]],
      purpose: ['', Validators.required],
      declaredMonthlyIncome: [null, [Validators.required, Validators.min(1)]],
      existingEmiObligations: [0, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {}

  onSubmit(): void {
    if (this.loanForm.invalid) {
      this.loanForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;
    this.successMessage = null;

    const formValue = this.loanForm.value;
    formValue.loanType = Number(formValue.loanType);

    this.loanService.submitApplication(formValue).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.successMessage = 'Loan application submitted successfully!';
        this.submittedApplicationId = response.id;
        this.submittedApplicationStatus = ApplicationStatus[response.status];
        this.loanForm.reset({ existingEmiObligations: 0 });
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.message;
      }
    });
  }
}
