export interface ApiResponse<T> {
  data: T;
  correlationId: string;
  timestamp: string;
}
