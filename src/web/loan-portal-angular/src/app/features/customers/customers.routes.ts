import { Routes } from '@angular/router';
import { CustomerRegistrationComponent } from './customer-registration/customer-registration.component';
import { CustomerListComponent } from './customer-list/customer-list.component';

export const CUSTOMER_ROUTES: Routes = [
  { path: '', component: CustomerListComponent },
  { path: 'register', component: CustomerRegistrationComponent }
];
