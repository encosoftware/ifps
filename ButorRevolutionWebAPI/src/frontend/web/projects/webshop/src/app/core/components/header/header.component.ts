import { Component, OnInit, Renderer2, ElementRef } from "@angular/core";
import { IGroupingCategoryWebshopViewModel } from '../../../home/models/home';
import { map, tap, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { HeaderService } from '../../services/header.service';
import { HeaderSharedService } from '../../../shared/services/header-shared.service';
import { BasketService } from '../../../basket/services/basket.service';
import { BasketDetailsViewModel } from '../../../basket/models/basket';
import { BasketSharedService } from '../../../shared/services/basket-shared.service';
import { LanguageSetService } from '../../../shared/services/language-set.service';
import { LoginService } from '../../../login/services/login.service';
import { TokenModel } from '../../../shared/models/auth';
import { Router } from '@angular/router';

@Component({
  selector: "app-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.scss"]
})
export class HeaderComponent implements OnInit {
  menuCategories: IGroupingCategoryWebshopViewModel[] = [];
  basketDetails: BasketDetailsViewModel;
  lngAll = this.lngService.languageValue;
  language: string;
  loggedIn: TokenModel;
  search: string;
  enteredButton = false;
  isMatMenuOpen = false;
  isMatMenu2Open = false;
  prevButtonTrigger;
  constructor(
    private headerService: HeaderService,
    private headerSharedService: HeaderSharedService,
    private basket: BasketService,
    private basketShared: BasketSharedService,
    private lngService: LanguageSetService,
    private loginService: LoginService,
    private router: Router,
    private ren: Renderer2
  ) { }

  ngOnInit() {
    this.getCategoriesByWebshop();
    this.getBasket();
    this.basketShared.basket.pipe(
      map(resp => this.basketDetails = resp),
    ).subscribe();

    this.lngAll.forEach((e) => {
      if (this.lngService.getLocalLanguageStorage() === e) {
        this.language = e;
      } else {
        this.language = this.lngService.defaultBrowserLng();
      }
    }
    );
    this.loginService.logedInSubject.pipe(
      map(login => this.loggedIn = login)
    ).subscribe();
  }

  getCategoriesByWebshop() {
    this.headerService.getCategoriesByWebshop().pipe(
      map(menu => {
        this.menuCategories = menu;
        this.headerSharedService.menu.next(menu);
      }),
    ).subscribe();
  }

  getBasket() {
    if (localStorage.getItem('basketId')) {
      this.basket.getBasketDetails(+localStorage.getItem('basketId')).pipe(
        map(details => this.basketShared.basket.next(details)),
      ).subscribe();
    }
  }

  switchLanguage(language: string) {
    const opposit = this.lngAll.find(x => (x !== language));
    this.language = opposit;
    this.lngService.setLanguage(opposit);
  }

  searchUnit(event: string) {
    this.headerService.searchFurnitureUnitInWebshop(event).pipe(
      debounceTime(500),
      distinctUntilChanged((x, y) => x === y),
      map(resp => this.headerSharedService.categories.next(resp)),
      tap(() => this.router.navigate(['/units']))
    ).subscribe();

  }


}
