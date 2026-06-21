import { Component } from '@angular/core';

@Component({
  selector: 'app-application-status',
  standalone: true,
  template: `
    <section class="panel placeholder">
      <span class="status-badge">Epic 4</span>
      <h2 class="section-title">Application Status</h2>
      <p class="muted">Controlled status transitions and application timeline views will be introduced here.</p>
    </section>
  `,
  styles: [`.placeholder { display: grid; gap: 12px; padding: 24px; }`]
})
export class ApplicationStatusComponent {}
