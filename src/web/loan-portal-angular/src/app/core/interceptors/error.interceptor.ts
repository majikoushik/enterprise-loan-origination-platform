import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An unexpected error occurred.';
      
      if (error.error instanceof ErrorEvent) {
        // Client-side or network error
        errorMessage = `A network error occurred: ${error.error.message}`;
      } else {
        // Backend returned an unsuccessful response code
        // Handle Problem Details format if available
        if (error.error && error.error.title) {
          errorMessage = error.error.title;
          
          if (error.error.detail) {
            errorMessage += `: ${error.error.detail}`;
          }

          if (error.error.errors) {
            // Flatten validation errors
            const validationErrors = [];
            for (const key in error.error.errors) {
              if (error.error.errors.hasOwnProperty(key)) {
                validationErrors.push(error.error.errors[key].join(', '));
              }
            }
            if (validationErrors.length > 0) {
              errorMessage += ` - ${validationErrors.join(' | ')}`;
            }
          }
        } else if (error.status === 0) {
          errorMessage = 'Unable to connect to the server. Please ensure the backend services are running.';
        } else {
          errorMessage = `Server returned code: ${error.status}, error message is: ${error.message}`;
        }
      }

      // We do not expose stack traces to UI components; we just return the safe message.
      console.error('Interceptor caught error:', error);
      return throwError(() => new Error(errorMessage));
    })
  );
};
