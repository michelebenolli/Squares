import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NotificationService } from '../services/notification.service';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(
    private notification: NotificationService,
    private router: Router) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((response: HttpErrorResponse) => {
        if (response.error) {
          switch (response.error.statusCode) {
            case 401:    
              this.router.navigateByUrl('/login').then(() => {
                this.notification.error(response.error.exception, 'Errore di autenticazione');
              });
              break;
            case 400 || 403:
              this.notification.error(response.error.exception);
              break;
            case 404:
              this.notification.error(response.error.exception, 'Pagina non trovata');
              break;
            case 500:
              this.notification.error(response.error.exception, 'Si è verificato un errore');
              break;
            default:
              const message = response.status === 0 ? 'Impossibile connettersi al server' : 'Si è verificato un errore';
              this.notification.error(response.error.exception, message);
              break;
          }
        }
        return throwError(() => response.error);
      })
    );
  }
}
