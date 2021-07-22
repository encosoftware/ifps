import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router, CanActivate, CanActivateChild } from '@angular/router';
import { Observable } from 'rxjs';
import { BasketSharedService } from '../../shared/services/basket-shared.service';
import { take, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BasketEmptyGuard  implements CanActivate, CanActivateChild {
  constructor(
    private basketService: BasketSharedService,
    private router: Router
  ) { }

  canActivate(route: ActivatedRouteSnapshot, router: RouterStateSnapshot):
        boolean | UrlTree | Promise<boolean | UrlTree> | Observable<boolean | UrlTree> {
        return this.basketService.basket.pipe(
            take(1),
            map((basket) => {
                const basketFull = !!basket;
                if (basketFull) {
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