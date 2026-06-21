import { Routes } from '@angular/router';
import { EligibilityCheckComponent } from './eligibility-check/eligibility-check.component';
import { EligibilityResultComponent } from './eligibility-result/eligibility-result.component';

export const ELIGIBILITY_ROUTES: Routes = [
  { path: 'check/:applicationId', component: EligibilityCheckComponent },
  { path: 'results/:applicationId', component: EligibilityResultComponent }
];
