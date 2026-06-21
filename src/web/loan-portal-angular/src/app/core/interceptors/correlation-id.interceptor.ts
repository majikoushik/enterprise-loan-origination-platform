import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { CorrelationIdService } from '../services/correlation-id.service';

export const correlationIdInterceptor: HttpInterceptorFn = (request, next) => {
  const correlationId = inject(CorrelationIdService).getSessionId();
  return next(request.clone({ setHeaders: { 'X-Correlation-ID': correlationId } }));
};
