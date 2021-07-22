import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, of } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class Errorhandling implements HttpInterceptor {
    constructor(
        private router: Router,
    ) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError((error) => {
                if (error instanceof HttpErrorResponse) {
                    switch (error.status) {
                        case 404: {
                            this.router.navigate(['/error/', { id: error.status }]);
                            return throwError(error);
                        }
                        case 403: {
                            this.router.navigate(['/error/', { id: error.status }]);
                            return throwError(error);
                        }
                        case 500: {
                            return throwError(error);
                        }
                        default: {
                            return throwError(error);
                        }
                    }
                }

            })

        );
    }
}
