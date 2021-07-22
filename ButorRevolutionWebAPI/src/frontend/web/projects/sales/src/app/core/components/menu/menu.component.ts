import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LanguageSetService } from '../../services/language-set.service';
import { MenuOpenService } from '../../services/menu-open.service';
import { ProfileComponent, IUserBasicInfo, IDropdownViewModel, SnackbarService } from 'butor-shared-lib';
import { AuthService } from '../../services/auth.service';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../store/selectors/core.selector';
import { take, tap, switchMap, catchError, map } from 'rxjs/operators';
import { TokenModel } from '../../models/auth';
import { UploadPicService } from '../../../shared/services/upload-pic.service';
import { of } from 'rxjs';
import { ClaimPolicyEnum } from '../../../shared/clients';
import { MatDialog } from '@angular/material/dialog';
import { UserPicUpdateService } from '../../../shared/services/user-pic-update.service';
import { NewPasswordComponent } from '../new-password/new-password.component';
import { CookieService } from 'ngx-cookie-service';


@Component({
  selector: 'butor-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  isMenuOpen: boolean;
  isMaterialOpenBigMenu = false;
  isMaterialMenuSmallOpen = false;
  @Output() isMenuOpenOutPut = new EventEmitter();
  language: string;
  lngAll = this.lngService.languageValue;
  token: TokenModel;
  containerName: string;
  ImageFileName: string;
  roleName: string;
  userName: string;
  userId: string;
  userBasic: IUserBasicInfo;
  languages: IDropdownViewModel[];
  countries: IDropdownViewModel[];
  previewUrl: string | ArrayBuffer;
  claimPolicyEnum = ClaimPolicyEnum;
  unanswear = this.menuService.getCountedUnansweredMessages();
  constructor(
    private translate: TranslateService,
    private lngService: LanguageSetService,
    private menuService: MenuOpenService,
    private authService: AuthService,
    private store: Store<any>,
    private picService: UploadPicService,
    public dialog: MatDialog,
    private picUpdateService: UserPicUpdateService,
    private snackBar: SnackbarService,
    private cookieService: CookieService
  ) { }

  ngOnInit() {
    this.loadData();
  }

  materialToggleMenu(): void {
    if (this.isMenuOpen) {
      this.isMaterialMenuSmallOpen = false;
      this.isMaterialOpenBigMenu = !this.isMaterialOpenBigMenu;
    } else {
      this.isMaterialMenuSmallOpen = !this.isMaterialMenuSmallOpen;
    }

  }

  switchLanguage(language: string) {
    this.language = language;
    this.lngService.setLanguage(language);
  }

  onToggleMenuBar() {
    this.isMenuOpen = !this.isMenuOpen;
    this.isMaterialMenuSmallOpen = false;
    this.isMaterialOpenBigMenu = false;
    this.menuService.changeMenuOpen(this.isMenuOpen);

  }

  logout() {
    this.authService.logoutUser(this.token.accessToken).subscribe();
  }

  openProfile() {
    const dialogRef = this.dialog.open(ProfileComponent, {
      width: '48rem',
      data: {
        userInfo: this.userBasic,
        countries: this.countries,
        languages: this.languages,
        picService: this.picService,
        roleName: this.roleName,
        translations: {
          name: this.translate.instant('profile.name'),
          nameReq: this.translate.instant('profile.nameReq'),
          language: this.translate.instant('profile.language'),
          languageReq: this.translate.instant('profile.languageReq'),
          phone: this.translate.instant('profile.phone'),
          phoneReq: this.translate.instant('profile.phoneReq'),
          password: this.translate.instant('profile.password'),
          email: this.translate.instant('profile.email'),
          emailReq: this.translate.instant('profile.emailReq'),
          cancel: this.translate.instant('profile.cancel'),
          save: this.translate.instant('profile.save'),
          address: this.translate.instant('profile.address'),
          addressReq: this.translate.instant('profile.addressReq'),
          city: this.translate.instant('profile.city'),
          cityReq: this.translate.instant('profile.cityReq'),
          postcode: this.translate.instant('profile.postcode'),
          postcodeReq: this.translate.instant('profile.postcodeReq'),
          country: this.translate.instant('profile.country'),
          countryReq: this.translate.instant('profile.countryReq'),
          emailFormat: this.translate.instant('profile.emailFormat'),
        }
      }
    });

    dialogRef.afterClosed().subscribe(res => {
      if (res && res !== -1) {
        this.menuService.updateUserProfile(res as IUserBasicInfo).subscribe(() => {
          this.authService.refresh(this.cookieService.get('key'), this.cookieService.get('key-r')).subscribe(result => {
            this.loadData();
            this.snackBar.customMessage(this.translate.instant('snackbar.success'));
          });
        });
      }
      if (res === -1) {
        const dialogRef2 = this.dialog.open(NewPasswordComponent, {
          width: '48rem'
        });
        dialogRef2.afterClosed().subscribe(result => {
          if (result) {
            this.menuService.changeUserPassword(result).subscribe(() => {
              this.snackBar.customMessage(this.translate.instant('snackbar.success'));
            });
          }
        });
      }
    });
  }

  loadData() {
    this.lngAll.forEach((e) => {
      if (this.lngService.getLocalLanguageStorage() === e) {
        this.language = e;
      }
    }
    );
    this.store.pipe(
      select(coreLoginT),
      take(1),
      tap((resp) => {
        this.roleName = resp.RoleName;
        this.userName = resp.UserName;
        this.userId = resp.UserId;

        this.token = ({
          accessToken: resp.accessToken,
          refreshToken: resp.refreshToken
        });
      }),
      switchMap(storeResp => {
        if (storeResp.ImageContainerName && storeResp.ImageFileName && storeResp) {
          return this.picService.getThumbnail(storeResp.ImageContainerName, storeResp.ImageFileName).pipe(
            tap((pic) => this.previewUrl = pic),
            catchError((err) => of(err.message))
          );
        }
        return of('image');
      }),
      switchMap(() => {
        return this.menuService.getUserBasicInfo().pipe(
          map((x) => this.userBasic = x),
        );
      }),
      switchMap(() => {
        return this.menuService.getLanguages().pipe(
          map((x) => this.languages = x),
        );
      }),
      switchMap(() => {
        return this.menuService.getCountries().pipe(
          map((x) => this.countries = x),
        );
      })
    ).subscribe();
    this.menuService.currentMenuOpen.subscribe(open => this.isMenuOpen = open);
  }
}
