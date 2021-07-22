import { Injectable, OnInit } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { Store, select } from '@ngrx/store';
import { Token } from '../store/actions/core.actions';
import { coreLogin } from '../store/selectors/core.selector';
import { tap, catchError, switchMap, filter, take } from 'rxjs/operators';
import { TokenModel } from '../models/auth';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    token: TokenModel = {
        accessToken: null,
        refreshToken: null
    };
    private refreshTokenInProgress = false;
    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(
        null
    );
    constructor(
        private authService: AuthService,
        private store: Store<any>
    ) {
        this.authService.logedInSubject.pipe(
            tap((token) => {
                if (token) {
                    this.token = token;
                    this.store.dispatch(new Token(token));
                }
            })
        ).subscribe();
        this.store.pipe(
            select(coreLogin),
            take(1),
            tap(resp => {
                if (resp) {
                    this.token = resp;
                }
            }),

        ).subscribe();
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (!(!!this.token.accessToken) && !(!!this.token.refreshToken)) {
            return next.handle(req);
        }

        const request = this.addAuthenticationToken(req);

        return next.handle(request).pipe(
            catchError((error) => {
                if (error instanceof HttpErrorResponse) {
                    if (
                        request.url.includes('refresh') ||
                        request.url.includes('login')
                    ) {
                        // We do another check to see if refresh token failed
                        // In this case we want to logout user and to redirect it to login page

                        if (request.url.includes('refresh')) {
                            this.authService.logoutUser(this.token.accessToken);
                        }

                        return throwError(error);
                    }
                    if (error.status !== 401) {
                        return throwError(error);
                    } else if (error.status === 401) {
                        if (this.refreshTokenInProgress) {
                            return this.refreshTokenSubject.pipe(
                                filter(result => result !== null),
                                take(1),
                                switchMap(() => next.handle(this.addAuthenticationToken(req)))
                            );
                        } else {
                            this.refreshTokenInProgress = true;
                            this.refreshTokenSubject.next(null);

                            return this.authService.refresh(this.token.accessToken, this.token.refreshToken)
                                .pipe(
                                    switchMap(resp => {
                                        this.store.dispatch(new Token(resp));
                                        this.token = resp;
                                        this.refreshTokenInProgress = false;
                                        this.refreshTokenSubject.next(resp);

                                        return next.handle(this.addAuthenticationToken(req));
                                    })
                                );
                        }
                    }
                }
                return throwError(error);

            })

        );
    }
    addAuthenticationToken(request: HttpRequest<any>): HttpRequest<any> {
        return request.clone({
            setHeaders: {
                Authorization: `Bearer ${this.token.accessToken}`,
                // 'Access-Control-Allow-Origin': '*',
            }
        });
    }
}
