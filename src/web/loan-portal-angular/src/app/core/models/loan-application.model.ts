export enum LoanType {
  PersonalLoan = 1,
  HomeLoan = 2,
  VehicleLoan = 3,
  EducationLoan = 4
}

export enum ApplicationStatus {
  Draft = 1,
  Submitted = 2,
  UnderReview = 3,
  EligibilityPassed = 4,
  EligibilityFailed = 5,
  Approved = 6,
  Rejected = 7,
  Cancelled = 8
}

export interface LoanApplicationRequest {
  customerId: string;
  loanType: LoanType;
  requestedAmount: number;
  requestedTenureInMonths: number;
  purpose: string;
  declaredMonthlyIncome: number;
  existingEmiObligations: number;
}

export interface LoanApplicationResponse {
  id: string;
  customerId: string;
  loanType: LoanType;
  requestedAmount: number;
  requestedTenureInMonths: number;
  purpose: string;
  declaredMonthlyIncome: number;
  existingEmiObligations: number;
  status: ApplicationStatus;
  createdAt: string;
  updatedAt: string;
}
