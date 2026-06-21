import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppShellComponent } from './layout/app-shell.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [AppShellComponent, RouterOutlet],
  template: `
    <app-shell>
      <router-outlet />
    </app-shell>
  `
})
export class AppComponent {}
