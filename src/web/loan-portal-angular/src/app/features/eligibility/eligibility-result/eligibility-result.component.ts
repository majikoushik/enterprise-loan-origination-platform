import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { EligibilityService } from '../../../core/services/eligibility.service';
import { EligibilityResult } from '../../../core/models/eligibility.model';

@Component({
  selector: 'app-eligibility-result',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './eligibility-result.component.html',
  styleUrls: ['./eligibility-result.component.css']
})
export class EligibilityResultComponent implements OnInit {
  applicationId: string | null = null;
  result: EligibilityResult | null = null;
  isLoading = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private eligibilityService: EligibilityService
  ) {}

  ngOnInit(): void {
    this.applicationId = this.route.snapshot.paramMap.get('applicationId');
    if (this.applicationId) {
      this.fetchResult();
    } else {
      this.error = 'Select a loan application to review its eligibility result.';
      this.isLoading = false;
    }
  }

  fetchResult(): void {
    if (!this.applicationId) return;

    this.eligibilityService.getResultByApplicationId(this.applicationId).subscribe({
      next: (res) => {
        this.result = res;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = err.message;
        this.isLoading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/loan-applications']);
  }
}
