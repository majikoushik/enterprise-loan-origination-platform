export interface RuleResult {
  ruleCode: string;
  ruleName: string;
  passed: boolean;
  actualValue: string;
  expectedValue: string;
  explanation: string;
}

export interface EligibilityResult {
  id: string;
  applicationId: string;
  customerId: string;
  decision: string;
  ruleVersion: string;
  evaluatedAt: string;
  requestedAmount: number;
  declaredMonthlyIncome: number;
  existingEmiObligations: number;
  debtToIncomeRatio: number;
  decisionSummary: string;
  ruleResults: RuleResult[];
}
