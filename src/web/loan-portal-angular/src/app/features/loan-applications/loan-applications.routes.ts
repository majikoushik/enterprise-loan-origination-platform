import { Routes } from '@angular/router';
import { LoanApplicationFormComponent } from './loan-application-form/loan-application-form.component';
import { LoanApplicationListComponent } from './loan-application-list/loan-application-list.component';

export const LOAN_APPLICATION_ROUTES: Routes = [
  { path: '', component: LoanApplicationListComponent },
  { path: 'new', component: LoanApplicationFormComponent }
];
