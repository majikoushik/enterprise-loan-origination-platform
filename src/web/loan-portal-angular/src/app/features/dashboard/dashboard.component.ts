import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  template: `
    <div class="page-shell">
      <section>
        <h2 class="section-title">Operations Dashboard</h2>
        <p class="muted">Enterprise loan origination demo covering customer onboarding, application workflow, eligibility checks, notification simulation, audit traceability, observability, Docker, CI, and Azure deployment planning.</p>
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

      <section class="panel architecture-summary">
        <div>
          <span class="summary-label">Architecture posture</span>
          <h3>Microservices-ready MVP with Azure production direction</h3>
          <p class="muted">The platform keeps local execution simple while preserving enterprise boundaries: service-owned data, API-first contracts, event-ready workflows, secure configuration, and operational diagnostics.</p>
        </div>
        <div class="capability-list" aria-label="Implemented platform capabilities">
          @for (capability of capabilities; track capability) {
            <span>{{ capability }}</span>
          }
        </div>
      </section>
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
      min-height: 132px;
    }

    .metric span,
    .metric small {
      color: #657089;
    }

    .metric strong {
      font-size: 1.55rem;
      color: #17324d;
    }

    .architecture-summary {
      display: grid;
      grid-template-columns: minmax(0, 1.2fr) minmax(280px, 0.8fr);
      gap: 24px;
      padding: 22px;
    }

    .summary-label {
      color: #17633a;
      font-size: 0.78rem;
      font-weight: 800;
      text-transform: uppercase;
    }

    h3 {
      margin: 6px 0 10px;
      color: #17324d;
      font-size: 1.2rem;
    }

    .capability-list {
      display: flex;
      flex-wrap: wrap;
      align-content: flex-start;
      gap: 8px;
    }

    .capability-list span {
      border: 1px solid #cbd8e6;
      border-radius: 999px;
      padding: 7px 10px;
      background: #f8fbfd;
      color: #40506b;
      font-size: 0.78rem;
      font-weight: 700;
    }

    @media (max-width: 980px) {
      .metric-grid {
        grid-template-columns: repeat(2, minmax(0, 1fr));
      }

      .architecture-summary {
        grid-template-columns: 1fr;
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
    { label: 'Customer Service', value: 'Live', note: 'Registration and profile API' },
    { label: 'Loan Workflow', value: 'Live', note: 'Submission and status rules' },
    { label: 'Eligibility Engine', value: 'Live', note: 'Demo rule evaluation' },
    { label: 'Audit Trail', value: 'Live', note: 'Centralized traceability' }
  ];

  protected readonly capabilities = [
    'API-first',
    'Correlation IDs',
    'Problem Details',
    'Health checks',
    'Docker Compose',
    'GitHub Actions',
    'Bicep blueprint',
    'Synthetic data only'
  ];
}
