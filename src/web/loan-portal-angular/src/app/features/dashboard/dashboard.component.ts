import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  template: `
    <div class="page-shell">
      <section>
        <h2 class="section-title">Operations Dashboard</h2>
        <p class="muted">Portfolio foundation for customer onboarding, loan application workflow, eligibility checks, notifications, and audit traceability.</p>
      </section>

      <div class="metric-grid">
        @for (metric of metrics; track metric.label) {
          <article class="panel metric">
            <span>{{ metric.label }}</span>
            <strong>{{ metric.value }}</strong>
            <small>{{ metric.note }}</small>
          </article>
        }
      </div>
    </div>
  `,
  styles: [`
    .metric-grid {
      display: grid;
      grid-template-columns: repeat(4, minmax(0, 1fr));
      gap: 16px;
    }

    .metric {
      display: grid;
      gap: 8px;
      padding: 18px;
    }

    .metric span,
    .metric small {
      color: #657089;
    }

    .metric strong {
      font-size: 1.55rem;
      color: #17324d;
    }

    @media (max-width: 980px) {
      .metric-grid {
        grid-template-columns: repeat(2, minmax(0, 1fr));
      }
    }

    @media (max-width: 560px) {
      .metric-grid {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class DashboardComponent {
  protected readonly metrics = [
    { label: 'Customer Service', value: 'Ready', note: 'Registration epic target' },
    { label: 'Loan Workflow', value: 'Planned', note: 'Submission and status' },
    { label: 'Eligibility Engine', value: 'Planned', note: 'Demo rule evaluation' },
    { label: 'Audit Trail', value: 'Planned', note: 'Traceability baseline' }
  ];
}
