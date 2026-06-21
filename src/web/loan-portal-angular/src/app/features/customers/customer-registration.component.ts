import { Component } from '@angular/core';

@Component({
  selector: 'app-customer-registration',
  standalone: true,
  template: `
    <section class="panel placeholder">
      <span class="status-badge">Epic 1</span>
      <h2 class="section-title">Customer Registration</h2>
      <p class="muted">Reactive onboarding form and Customer API integration will be implemented in the next customer epic.</p>
    </section>
  `,
  styles: [`.placeholder { display: grid; gap: 12px; padding: 24px; }`]
})
export class CustomerRegistrationComponent {}
