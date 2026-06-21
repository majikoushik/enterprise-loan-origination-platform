import { Component } from '@angular/core';

@Component({
  selector: 'app-loan-application-form',
  standalone: true,
  template: `
    <section class="panel placeholder">
      <span class="status-badge">Epic 2</span>
      <h2 class="section-title">Loan Application</h2>
      <p class="muted">Application capture, validation, and submission workflow will be added after customer registration.</p>
    </section>
  `,
  styles: [`.placeholder { display: grid; gap: 12px; padding: 24px; }`]
})
export class LoanApplicationFormComponent {}
