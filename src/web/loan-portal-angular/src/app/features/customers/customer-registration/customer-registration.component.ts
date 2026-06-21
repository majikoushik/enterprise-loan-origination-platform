import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CustomerService } from '../../../core/services/customer.service';
import { EmploymentType } from '../../../core/models/customer.model';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-customer-registration',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './customer-registration.component.html',
  styleUrls: ['./customer-registration.component.css']
})
export class CustomerRegistrationComponent {
  registrationForm: FormGroup;
  isLoading = false;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  
  // Make enum available in template
  employmentTypes = [
    { value: EmploymentType.Salaried, label: 'Salaried' },
    { value: EmploymentType.SelfEmployed, label: 'Self Employed' },
    { value: EmploymentType.Unemployed, label: 'Unemployed' },
    { value: EmploymentType.Other, label: 'Other' }
  ];

  constructor(
    private fb: FormBuilder,
    private customerService: CustomerService,
    private router: Router
  ) {
    this.registrationForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      mobileNumber: ['', [Validators.required, Validators.pattern(/^\+?[1-9]\d{1,14}$/)]],
      dateOfBirth: ['', Validators.required],
      employmentType: ['', Validators.required],
      monthlyIncome: [null, [Validators.required, Validators.min(1)]],
      existingMonthlyObligations: [0, [Validators.required, Validators.min(0)]]
    });
  }

  onSubmit() {
    if (this.registrationForm.invalid) {
      this.registrationForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;
    this.successMessage = null;

    const formValue = this.registrationForm.value;
    // Format the date properly
    formValue.dateOfBirth = new Date(formValue.dateOfBirth).toISOString();
    formValue.employmentType = Number(formValue.employmentType);

    this.customerService.registerCustomer(formValue).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.successMessage = `Customer ${response.fullName} registered successfully!`;
        this.registrationForm.reset();
        setTimeout(() => {
          this.router.navigate(['/customers']);
        }, 2000);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.message;
      }
    });
  }
}
