import { Component, OnInit } from '@angular/core';
import { BasketService } from '../services/basket.service';
import { BasketDetailsViewModel, ShippingServiceListViewModel } from '../models/basket';
import { map, finalize, debounceTime, tap, switchMap, distinctUntilChanged, take, filter } from 'rxjs/operators';
import { BasketCreateModel } from '../../shared/models/shared';
import { BasketSharedService } from '../../shared/services/basket-shared.service';
import { LoginService } from '../../login/services/login.service';
import { TokenModel } from '../../shared/models/auth';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../login/store/selectors/core.selector';
import { Subscription, combineLatest } from 'rxjs';
import { Router } from '@angular/router';
import { SnackbarService } from 'butor-shared-lib';

@Component({
  selector: 'app-baskets',
  templateUrl: './baskets.component.html',
  styleUrls: ['./baskets.component.scss']
})
export class BasketsComponent implements OnInit {
  basketDetails: BasketDetailsViewModel;
  isLoading = false;
  shipping: ShippingServiceListViewModel[];
  selectedShipping = 1;
  select: ShippingServiceListViewModel;
  choose = true;
  loggedIn: TokenModel;
  userId: number;
  subscribe: Subscription;
  constructor(
    private basket: BasketService,
    private basketShared: BasketSharedService,
    private loginService: LoginService,
    private store: Store<any>,
    private router: Router,
    private snackbar: SnackbarService


  ) { }

  ngOnInit() {
    if (localStorage.getItem('basketId')) {
      this.isLoading = true;

      this.basket.getBasketDetails(+localStorage.getItem('basketId')).pipe(
        map(resp => this.basketShared.basket.next(resp)),
        finalize(() => this.isLoading = false)
      ).subscribe();
    }

    this.subscribe = combineLatest([
      this.basket.getShippingServicesForDropdown().pipe(
        map(x => this.shipping = x),
        tap(_ => this.select = this.shipping.find(x => x.id === this.selectedShipping))
      ),
      this.basketShared.basket.pipe(
        map(resp => this.basketDetails = resp)
      ),
      this.loginService.logedInSubject.pipe(
        map(login => this.loggedIn = login)
      ),
      this.store.pipe(
        select(coreLoginT),
        take(1),
        map(res => this.userId = +res.UserId),
      )
    ]).subscribe((
      [shipping, basketDetails, login, store]: [ShippingServiceListViewModel[], BasketDetailsViewModel, TokenModel, number]) => { });
  }

  update(quantity, index: number) {
    const basket: BasketCreateModel = ({
      customerId: this.userId ? null : this.userId,
      orderedFurnitureUnit:
        this.basketDetails.orderedFurnitureUnits.map(x =>
          ({
            furnitureUnitId: x.furnitureUnitId,
            quantity: x.quantity,
          })),
      deliveryPrice: this.select.price,
      serviceId: this.selectedShipping,

    });

    if (quantity >= 1) {
      this.basket.updateBasketItem(+localStorage.getItem('basketId'), basket, false).pipe(
        debounceTime(500),
        distinctUntilChanged((x, y) => x === y),
        switchMap(_ =>
          this.basket.getBasketDetails(+localStorage.getItem('basketId')).pipe(
            map(resp => this.basketShared.basket.next(resp)),
            finalize(() => this.isLoading = false)
          )
        )
      ).subscribe();
    } else if (quantity <= 0) {
      this.basketDetails.orderedFurnitureUnits[index].quantity = 1;
    }
  }

  updateCheckout() {
    const basket: BasketCreateModel = ({
      customerId: this.userId ? null : this.userId,
      orderedFurnitureUnit:
        this.basketDetails.orderedFurnitureUnits.map(x =>
          ({
            furnitureUnitId: x.furnitureUnitId,
            quantity: x.quantity,
          }))
      ,
      deliveryPrice: this.select.price
    });
    this.basket.updateBasketItem(+localStorage.getItem('basketId'), basket, false).pipe(
      debounceTime(500),
      distinctUntilChanged((x, y) => x === y),
      switchMap(_ =>
        this.basket.getBasketDetails(+localStorage.getItem('basketId')).pipe(
          map(resp => this.basketShared.basket.next(resp)),
          finalize(() => this.isLoading = false)
        )
      )
    ).subscribe();
  }

  delete(id: string) {
    this.basket.deleteBasketItem(+localStorage.getItem('basketId'), id).pipe(
      switchMap(x =>
        this.basket.getBasketDetails(+localStorage.getItem('basketId')).pipe(
          map(resp => {
            if (resp.orderedFurnitureUnits.length > 0) {
              this.basketShared.basket.next(resp);
            } else {
              this.basketShared.basket.next(null);
              this.router.navigate(['/']);

            }
          })
        )
      ),
      finalize(() => this.snackbar.deleted())
    ).subscribe();
  }
  addShippingPrice() {
    this.select = this.shipping.find(x => x.id === this.selectedShipping);
  }

  shippingChoose() {
    this.choose = !this.choose;
    if (!this.choose) {
      this.selectedShipping = 1;
      this.select = this.shipping.find(x => x.id === this.selectedShipping);

    }
  }
}
