import { Component, OnInit } from '@angular/core';
import { BasketService } from '../../basket/services/basket.service';
import { BasketSharedService } from '../../shared/services/basket-shared.service';
import { map, finalize, switchMap, take, tap } from 'rxjs/operators';
import { BasketDetailsViewModel } from '../../basket/models/basket';
import { LoginService } from '../../login/services/login.service';
import { TokenModel } from '../../shared/models/auth';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../login/store/selectors/core.selector';
import { NgForm } from '@angular/forms';
import { PurchaseService } from '../services/purchase.service';
import { BasketPurchaseViewModel, CountryListSelectModel } from '../models/purchase';
import { AccountService } from '../../account-history/services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-purchase',
  templateUrl: './purchase.component.html',
  styleUrls: ['./purchase.component.scss']
})
export class PurchaseComponent implements OnInit {

  isLoading = false;
  basketDetails: BasketDetailsViewModel;
  loggedIn: TokenModel;
  userName: string;
  email: string;
  emailSend = false;
  termsCond = false;
  myself = false;
  postalCode: number;
  country: number;
  city: string;
  address: string;
  note: string;
  billingSame = false;
  postalCodeElse: string;
  countryElse: number;
  cityElse: string;
  addressElse: string;
  taxNumber: string;
  id: number;
  countries: CountryListSelectModel[];

  constructor(
    private basket: BasketService,
    private basketShared: BasketSharedService,
    private purchaseService: PurchaseService,
    private loginService: LoginService,
    private store: Store<any>,
    private account: AccountService,
    private router: Router


  ) { }

  ngOnInit() {
    this.isLoading = true;
    this.purchaseService.getCountries().subscribe(res => this.countries = res);
    this.basketShared.basket.pipe(
      map(resp => this.basketDetails = resp),
      finalize(() => { this.isLoading = false; })
    ).subscribe();

    this.loginService.logedInSubject.pipe(
      map(login => this.loggedIn = login),
      switchMap(login =>
        this.store.pipe(
          select(coreLoginT),
          take(1),
          tap((resp) => {
            this.id = +resp.UserId;
          }),
          switchMap(() =>
          this.account.getUserBasicInfo(this.id).pipe(
            map(resp => {
              this.userName = resp.name;
              this.email = resp.email;
              this.postalCode = resp.postCode;
              this.country = resp.countryId;
              this.city = resp.city;
              this.address = resp.address;
  
            })
          )
        )
        )
      )).subscribe();

  }

  purchase(f: NgForm) {
    const purchaseModel: BasketPurchaseViewModel = ({
      customerId: this.id ? this.id : null,
      name: this.userName,
      emailAddress: this.email,
      note: this.note,
      taxNumber: this.taxNumber ? this.taxNumber : null,
      gaveEmailConsent: this.emailSend,
      delieveryAddress: ({
        address: this.address,
        postCode: +this.postalCode,
        city: this.city,
        countryId: this.country,
      }),
      billingAddress: ({
        address: this.billingSame ? this.addressElse : this.address,
        postCode: this.billingSame ? +this.postalCodeElse : +this.postalCode,
        city: this.billingSame ? this.cityElse : this.city,
        countryId: this.billingSame ? this.countryElse : this.country,
      }),
    })
    this.purchaseService.purchaseBasket(+localStorage.getItem('basketId'), purchaseModel).subscribe(
      resp => {
        localStorage.removeItem('basketId');
        this.basketShared.basket.next(null);
        this.router.navigate(['purchase/success']);

      }
    );
  }

}
