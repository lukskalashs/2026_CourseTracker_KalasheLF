import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { BusyService } from '../services/busy-service';
import { catchError, delay, finalize, throttleTime, throwError } from 'rxjs';
import { ToastService } from '../services/toast-service';
import { Router } from '@angular/router';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toastService = inject(ToastService)
  // const route = inject(Router)

  return next(req).pipe(
    catchError(error => {
      if(error){
        const errorMessage = error.error?.title 
          || error.error
          || error.message || 'An unexpected error happened';
      }
      return throwError(() => error)
    })
  )
};
