import { Injectable } from '@angular/core';
import {
  ApiAccountLoginClient, ApiAccountRegisterClient, ApiAccountLogoutClient, ApiAccountRefreshClient, ApiUsersEmailClient,
  ApiAccountConfirmClient, ApiAccountPasswordResetClient, ApiAccountPasswordChangeClient, ApiClaimsClient, LoginDto,
  UserCreateDto, TokenDto, PasswordChangeDto, LogoutTokenDto
} from '../../shared/clients';
import { Store } from '@ngrx/store';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { JwtTokenModel, TokenModel, LoginViewModel } from '../../shared/models/auth';
import { tap, map } from 'rxjs/operators';
import { Token, LoginToken } from '../store/actions/core.actions';
import * as jwt_decode from 'jwt-decode';
import { BasketSharedService } from '../../shared/services/basket-shared.service';


@Injectable({
  providedIn: 'root'
})
export class LoginService {


  jwtToken: JwtTokenModel;
  token: TokenModel;
  logedInSubject: BehaviorSubject<TokenModel> = new BehaviorSubject(null);
  private oldDateObj = new Date();
  private newDateObj = new Date();
  private refreshTime = new Date(this.newDateObj.setTime(this.oldDateObj.getTime() + (10 * 60 * 1000)));

  constructor(
    private login: ApiAccountLoginClient,
    private registration: ApiAccountRegisterClient,
    private logout: ApiAccountLogoutClient,
    private refreshToken: ApiAccountRefreshClient,
    private emailValid: ApiUsersEmailClient,
    private confirmEmail: ApiAccountConfirmClient,
    private resetEmail: ApiAccountPasswordResetClient,
    private passwordChange: ApiAccountPasswordChangeClient,
    private claimList: ApiClaimsClient,
    private store: Store<any>,
    private cookieService: CookieService,
    private router: Router,
    private basketShared: BasketSharedService,

  ) {
    if (!!this.cookieService.get('key') && !!this.cookieService.get('key-r')) {
      this.logedInSubject = new BehaviorSubject<TokenModel>({
        accessToken: this.cookieService.get('key'),
        refreshToken: this.cookieService.get('key-r')
      });
      this.getTokenFromCookies();
    } else {
      this.logedInSubject = new BehaviorSubject<TokenModel>(null);
      localStorage.removeItem('m-claim');
    }
  }

  loginUser(login: LoginViewModel): Observable<TokenModel> {
    const dto = new LoginDto(login);
    return this.login.login(dto).pipe(
      map(token => ({
        accessToken: token.accessToken,
        refreshToken: token.refreshToken
      })
      ),
      tap((token) => {
        this.cookieService.set('key', token.accessToken, this.refreshTime, '/', '', false, 'Strict');
        this.cookieService.set('key-r', token.refreshToken, this.refreshTime, '/', '', false, 'Strict');
        this.getTokenFromCookies();
      })
    );
  }

  registrationUser(name: string, email: string, password: string, gaveEmailConsent: boolean): Observable<void> {
    const dto = new UserCreateDto({
      name,
      email,
      password,
      gaveEmailConsent
    });
    return this.registration.register(dto);
  }

  logoutUser(accessToken: string): Observable<void> {
    const dto = new LogoutTokenDto({
      accessToken
    });
    return this.logout.logout(dto).pipe(
      tap(_ => {
        this.cookieService.deleteAll();
        this.logedInSubject.next(null);
        this.store.dispatch(new Token({
          accessToken: '',
          refreshToken: ''
        }));
        this.store.dispatch(new LoginToken({
          accessToken: '',
          refreshToken: '',
          Email: '',
          ImageContainerName: '',
          ImageFileName: '',
          BasketId: '',
          Language: '',
          RoleName: '',
          UserName: '',
          IFPSClaim: [],
          UserId: '',
        }));
        this.basketShared.basket.next(null);
        localStorage.clear();
        this.router.navigate(['/login']);
        localStorage.removeItem('basketId');
      })
    );
  }

  refresh(accessToken: string, refreshToken: string): Observable<TokenModel> {
    const dto = new TokenDto({
      accessToken,
      refreshToken
    });
    return this.refreshToken.refresh(dto).pipe(
      map(token => ({
        accessToken: token.accessToken,
        refreshToken: token.refreshToken
      })),
      tap((token) => {
        this.cookieService.set('key', token.accessToken, this.refreshTime, '/', '', false, 'Strict');
        this.cookieService.set('key-r', token.refreshToken, this.refreshTime, '/', '', false, 'Strict');
        this.getTokenFromCookies();
      })
    );
  }

  emailValidation(email): Observable<boolean> {
    return this.emailValid.validateEmail(email);
  }

  confirmedEmail(userId: number, token: string): Observable<void> {
    return this.confirmEmail.confirmEmail(userId, token);
  }

  resetPassword(email: string): Observable<void> {
    return this.resetEmail.resetPassword(email);
  }

  changePassword(userId: number, token: string | null, password: string): Observable<void> {
    let pChangeDto = new PasswordChangeDto({
      password
    });

    return this.passwordChange.changePassword(userId, token, pChangeDto);
  }


  getTokenFromCookies() {
    const token = this.cookieService.get('key');
    const refresh = this.cookieService.get('key-r');
    if (!!token && !!refresh) {
      this.jwtToken = jwt_decode(token);
      this.logedInSubject.next({
        accessToken: token,
        refreshToken: refresh
      });
      this.store.dispatch(new Token({
        accessToken: token,
        refreshToken: refresh
      }));
      this.store.dispatch(new LoginToken({
        accessToken: token,
        refreshToken: refresh,
        Email: this.jwtToken.Email,
        ImageContainerName: this.jwtToken.ImageContainerName,
        ImageFileName: this.jwtToken.ImageFileName,
        Language: this.jwtToken.Language,
        BasketId: this.jwtToken.BasketId,
        RoleName: this.jwtToken.RoleName,
        UserName: this.jwtToken.UserName,
        IFPSClaim: this.jwtToken.IFPSClaim,
        UserId: this.jwtToken.UserId,
      }));
      localStorage.setItem('basketId', this.jwtToken.BasketId);
    }
  }
}
