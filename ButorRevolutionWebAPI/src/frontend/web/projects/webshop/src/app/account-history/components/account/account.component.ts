import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../../login/store/selectors/core.selector';
import { take, map, switchMap, tap, finalize } from 'rxjs/operators';
import { CountryListSelectModel } from '../../../purchase/models/purchase';
import { NgForm } from '@angular/forms';
import { IUserBasicInfo } from '../../models/account';
import { LanguageTypeEnum } from '../../../shared/clients';
import { LanguageSetService } from '../../../shared/services/language-set.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {
  userName: string;
  email: string;
  password: string;
  passwordToShow = '***********';
  passwordCheck: string;
  postalCode: number;
  country: number;
  city: string;
  address: string;
  userId: number;
  countries: CountryListSelectModel[];
  edit = true;
  passwordNew: string;
  isLoading = false;

  constructor(
    private account: AccountService,
    private store: Store<any>,
    private lngService: LanguageSetService,

  ) { }

  ngOnInit() {
    this.account.getCountries().subscribe(res => this.countries = res);
    this.isLoading = true;
    this.store.pipe(
      select(coreLoginT),
      take(1),
      tap(res => this.userId = +res.UserId),
      switchMap(() =>
        this.account.getUserBasicInfo(this.userId).pipe(
          map(resp => {
            this.userName = resp.name;
            this.email = resp.email;
            this.postalCode = resp.postCode;
            this.country = resp.countryId;
            this.city = resp.city;
            this.address = resp.address;

          })
        )
      ),
      finalize(() => { this.isLoading = false; })
    ).subscribe();



  }

  profileChange(profile: NgForm) {
    const user: IUserBasicInfo = ({
      name: this.userName,
      email: this.email,
      countryId: this.country,
      postCode: this.postalCode,
      city: this.city,
      address: this.address,
      languageId: LanguageTypeEnum[this.lngService.getLocalLanguageStorage()],
      currentPassword: this.password,
      newPassword: this.passwordNew,
    });
    this.account.updateUserProfile(this.userId, user).subscribe();
  }

}
