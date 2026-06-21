import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const apiErrorInterceptor: HttpInterceptorFn = (request, next) =>
  next(request).pipe(
    catchError((error: HttpErrorResponse) => {
      const userSafeError = {
        status: error.status,
        title: error.error?.title ?? 'Request failed',
        detail: error.error?.detail ?? 'The platform request could not be completed.',
        correlationId: error.error?.correlationId ?? error.headers.get('X-Correlation-ID')
      };

      return throwError(() => userSafeError);
    })
  );
