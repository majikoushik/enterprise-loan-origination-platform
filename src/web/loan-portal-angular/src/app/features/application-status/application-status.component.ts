import { Component } from '@angular/core';

@Component({
  selector: 'app-application-status',
  standalone: true,
  template: `
    <section class="panel status-overview">
      <span class="status-badge">Workflow Controlled</span>
      <h2 class="section-title">Application Status</h2>
      <p class="muted">Loan applications move through documented statuses with domain-enforced transitions and persistent timeline history.</p>
      <div class="status-list" aria-label="Supported application statuses">
        @for (status of statuses; track status) {
          <span>{{ status }}</span>
        }
      </div>
    </section>
  `,
  styles: [`
    .status-overview {
      display: grid;
      gap: 14px;
      padding: 24px;
    }

    .status-list {
      display: flex;
      flex-wrap: wrap;
      gap: 8px;
    }

    .status-list span {
      border: 1px solid #cbd8e6;
      border-radius: 999px;
      padding: 7px 10px;
      background: #f8fbfd;
      color: #40506b;
      font-size: 0.78rem;
      font-weight: 700;
    }
  `]
})
export class ApplicationStatusComponent {
  protected readonly statuses = [
    'Draft',
    'Submitted',
    'UnderReview',
    'EligibilityPassed',
    'EligibilityFailed',
    'Approved',
    'Rejected',
    'Cancelled'
  ];
}
