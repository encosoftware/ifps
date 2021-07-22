import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate, Router, CanActivateChild } from '@angular/router';
import { Observable, combineLatest } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { map, take } from 'rxjs/operators';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../store/selectors/core.selector';

@Injectable({
  providedIn: 'root'
})
export class ClaimGuard implements CanActivate, CanActivateChild {

  constructor(
    private authService: AuthService,
    private router: Router,
    private store: Store<any>,
  ) { }

  canActivate(route: ActivatedRouteSnapshot, router: RouterStateSnapshot):
    boolean | UrlTree | Promise<boolean | UrlTree> | Observable<boolean | UrlTree> {
    return combineLatest([
      this.authService.logedInSubject.pipe(
        take(1),
        map((inOut) => {
          return !!inOut;
        }),
      ),
      this.store.pipe(
        select(coreLoginT),
        take(1),
        map((inOut) => {
          return inOut.IFPSClaim;
        }))
    ]).pipe(
      map(([logedIn, myClaimList]: ([boolean, string[]])) => {
        if (!route.data.claims) {
          return true;
        } else if (!!myClaimList.find(claim => claim === route.data.claims) && logedIn) {
          return true;
        } else if (!logedIn) {
          return this.router.createUrlTree(['/login']);
        } else {
          return this.router.createUrlTree(['/sales/dashboard']);
        }
      }));
  }

  canActivateChild(route: ActivatedRouteSnapshot, router: RouterStateSnapshot):
    boolean | UrlTree | Promise<boolean | UrlTree> | Observable<boolean | UrlTree> {
    return this.canActivate(route, router);
  }
}
