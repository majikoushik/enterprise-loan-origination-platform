import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { EligibilityService } from '../../../core/services/eligibility.service';

@Component({
  selector: 'app-eligibility-check',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './eligibility-check.component.html',
  styleUrls: ['./eligibility-check.component.css']
})
export class EligibilityCheckComponent implements OnInit {
  applicationId: string | null = null;
  isLoading = false;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private eligibilityService: EligibilityService
  ) {}

  ngOnInit(): void {
    this.applicationId = this.route.snapshot.paramMap.get('applicationId');
  }

  runEligibilityCheck(): void {
    if (!this.applicationId) return;

    this.isLoading = true;
    this.error = null;

    this.eligibilityService.checkEligibility(this.applicationId).subscribe({
      next: (result) => {
        this.isLoading = false;
        this.router.navigate(['/eligibility/results', this.applicationId]);
      },
      error: (err) => {
        this.isLoading = false;
        this.error = err.message;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/loan-applications']);
  }
}
