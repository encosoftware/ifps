import { Injectable } from '@angular/core';
import {
  ApiAccountLoginClient,
  ApiAccountRegisterClient,
  ApiAccountLogoutClient,
  ApiAccountRefreshClient,
  LoginDto,
  TokenDto,
  ApiUsersEmailClient,
  ApiAccountConfirmClient,
  ApiAccountPasswordResetClient,
  ApiAccountPasswordChangeClient,
  PasswordChangeDto,
  ApiClaimsClient,
  UserCreateDto,
} from '../../shared/clients';
import { Observable, BehaviorSubject } from 'rxjs';
import { TokenModel, LoginViewModel, JwtTokenModel } from '../models/auth';
import { map, tap } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { Token, Claims, LoginToken } from '../store/actions/core.actions';
import * as jwt_decode from 'jwt-decode';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  jwtToken: JwtTokenModel;
  token: TokenModel;
  logedInSubject: BehaviorSubject<TokenModel>;
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
    private router: Router
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
    const dto = new TokenDto({
      accessToken
    });
    return this.logout.logout(dto).pipe(
      tap(_ => {
        this.cookieService.deleteAll();
        this.logedInSubject.next(null);
        localStorage.removeItem('m-claim');
        this.store.dispatch(new Token(null));
        this.router.navigate(['/login']);
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

  getClaimsList(): Observable<string[]> {
    return this.claimList.getClaimsList();
  }

  getTokenFromCookies() {
    const token = this.cookieService.get('key');
    const refresh = this.cookieService.get('key-r');
    if (!!token && !!refresh) {
      this.jwtToken = jwt_decode(token);
      localStorage.setItem('m-claim', JSON.stringify(this.jwtToken.IFPSClaim));
      this.store.dispatch(new Claims(this.jwtToken.IFPSClaim));
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
        RoleName: this.jwtToken.RoleName,
        UserName: this.jwtToken.UserName,
        IFPSClaim: this.jwtToken.IFPSClaim,
        UserId: this.jwtToken.UserId,
        CompanyId: this.jwtToken.CompanyId
      }));
    }
  }
}
