import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate, CanActivateChild, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { take, map } from 'rxjs/operators';
import { LoginService } from '../../login/services/login.service';

@Injectable({
  providedIn: 'root'
})
export class CheckLoginGuard implements CanActivate, CanActivateChild {

  constructor(
    private authService: LoginService,
    private router: Router
  ) { }

  canActivate(route: ActivatedRouteSnapshot, router: RouterStateSnapshot):
        boolean | UrlTree | Promise<boolean | UrlTree> | Observable<boolean | UrlTree> {
        return this.authService.logedInSubject.pipe(
            take(1),
            map((inOut) => {
                const isAuth = !!inOut;
                if (isAuth) {
                    return true;
                }
                return this.router.createUrlTree(['/']);
            }),
        );
    }
    canActivateChild(route: ActivatedRouteSnapshot, router: RouterStateSnapshot):
    boolean | UrlTree | Promise<boolean | UrlTree> | Observable<boolean | UrlTree> {
        return this.canActivate(route, router);
    }
}