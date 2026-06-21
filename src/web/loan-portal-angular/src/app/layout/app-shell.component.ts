import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

interface NavigationItem {
  label: string;
  route: string;
}

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  template: `
    <div class="app-layout">
      <aside class="sidebar">
        <div class="brand">
          <span class="brand-mark">LO</span>
          <div>
            <strong>Loan Origination</strong>
            <span>Enterprise Portal</span>
          </div>
        </div>

        <nav aria-label="Primary navigation">
          @for (item of navigationItems; track item.route) {
            <a [routerLink]="item.route" routerLinkActive="active">{{ item.label }}</a>
          }
        </nav>
      </aside>

      <main class="content">
        <header class="topbar">
          <div>
            <p class="eyebrow">Banking Platform MVP</p>
            <h1>Loan Origination Workspace</h1>
          </div>
          <span class="status-badge">Foundation Ready</span>
        </header>

        <section class="main-panel">
          <ng-content />
        </section>
      </main>
    </div>
  `,
  styles: [`
    .app-layout {
      display: grid;
      grid-template-columns: 280px minmax(0, 1fr);
      min-height: 100vh;
    }

    .sidebar {
      display: flex;
      flex-direction: column;
      gap: 28px;
      border-right: 1px solid #dce3ee;
      background: #ffffff;
      padding: 28px 20px;
    }

    .brand {
      display: flex;
      align-items: center;
      gap: 12px;
    }

    .brand-mark {
      display: grid;
      width: 42px;
      height: 42px;
      place-items: center;
      border-radius: 8px;
      background: #17324d;
      color: #ffffff;
      font-weight: 800;
    }

    .brand strong,
    .brand span {
      display: block;
    }

    .brand span {
      color: #657089;
      font-size: 0.82rem;
    }

    nav {
      display: grid;
      gap: 6px;
    }

    nav a {
      border-radius: 8px;
      padding: 11px 12px;
      color: #40506b;
      font-weight: 650;
    }

    nav a.active,
    nav a:hover {
      background: #edf3f8;
      color: #17324d;
    }

    .content {
      min-width: 0;
      padding: 24px;
    }

    .topbar {
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 20px;
      margin-bottom: 24px;
    }

    .eyebrow {
      margin: 0 0 4px;
      color: #657089;
      font-size: 0.82rem;
      font-weight: 700;
      text-transform: uppercase;
    }

    h1 {
      margin: 0;
      font-size: 1.7rem;
      line-height: 1.2;
    }

    .main-panel {
      max-width: 1180px;
    }

    @media (max-width: 820px) {
      .app-layout {
        grid-template-columns: 1fr;
      }

      .sidebar {
        border-right: 0;
        border-bottom: 1px solid #dce3ee;
      }

      nav {
        grid-template-columns: repeat(2, minmax(0, 1fr));
      }

      .topbar {
        align-items: flex-start;
        flex-direction: column;
      }
    }
  `]
})
export class AppShellComponent {
  protected readonly navigationItems: NavigationItem[] = [
    { label: 'Dashboard', route: '/dashboard' },
    { label: 'Customer Registration', route: '/customers/register' },
    { label: 'Loan Application', route: '/loan-applications/new' },
    { label: 'Eligibility Result', route: '/eligibility/result' },
    { label: 'Application Status', route: '/applications/status' },
    { label: 'Audit Trail', route: '/audit-trail' },
    { label: 'Notifications', route: '/notifications' }
  ];
}
