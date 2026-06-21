export enum EmploymentType {
  Salaried = 1,
  SelfEmployed = 2,
  Unemployed = 3,
  Other = 4
}

export interface CustomerRegistrationRequest {
  fullName: string;
  email: string;
  mobileNumber: string;
  dateOfBirth: string; // ISO date string
  employmentType: EmploymentType;
  monthlyIncome: number;
  existingMonthlyObligations: number;
}

export interface CustomerResponse {
  id: string;
  fullName: string;
  email: string;
  mobileNumber: string;
  dateOfBirth: string;
  employmentType: EmploymentType;
  monthlyIncome: number;
  existingMonthlyObligations: number;
  createdAt: string;
}
