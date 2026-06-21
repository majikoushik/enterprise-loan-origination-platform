import { Component } from '@angular/core';

@Component({
  selector: 'app-eligibility-result',
  standalone: true,
  template: `
    <section class="panel placeholder">
      <span class="status-badge">Epic 3</span>
      <h2 class="section-title">Eligibility Result</h2>
      <p class="muted">Rule outcomes, debt-to-income checks, and decision explanation will be displayed here.</p>
    </section>
  `,
  styles: [`.placeholder { display: grid; gap: 12px; padding: 24px; }`]
})
export class EligibilityResultComponent {}
