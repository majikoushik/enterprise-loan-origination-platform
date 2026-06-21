import { Routes } from '@angular/router';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { LoanApplicationFormComponent } from './features/loan-applications/loan-application-form.component';
import { EligibilityResultComponent } from './features/eligibility/eligibility-result.component';
import { ApplicationStatusComponent } from './features/application-status/application-status.component';
import { AuditTrailComponent } from './features/audit-trail/audit-trail.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
  { path: 'dashboard', component: DashboardComponent, title: 'Dashboard' },
  { path: 'customers', loadChildren: () => import('./features/customers/customers.routes').then(m => m.CUSTOMER_ROUTES) },
  { path: 'loan-applications/new', component: LoanApplicationFormComponent, title: 'Loan Application' },
  { path: 'eligibility/result', component: EligibilityResultComponent, title: 'Eligibility Result' },
  { path: 'applications/status', component: ApplicationStatusComponent, title: 'Application Status' },
  { path: 'audit-trail', component: AuditTrailComponent, title: 'Audit Trail' },
  { path: '**', redirectTo: 'dashboard' }
];
