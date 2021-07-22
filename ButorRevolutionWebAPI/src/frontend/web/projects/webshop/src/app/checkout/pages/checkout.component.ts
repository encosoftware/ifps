import { Component, OnInit } from '@angular/core';
import { TokenModel, LoginViewModel } from '../../shared/models/auth';
import { NgForm } from '@angular/forms';
import { LoginService } from '../../login/services/login.service';
import { tap, catchError, switchMap, filter, finalize } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { Router } from '@angular/router';
import { BasketService } from '../../basket/services/basket.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  emailLogin = 'enco@enco.hu';
  passwordLogin = 'password';
  isLoading = false;
  passOrUser = false;
  token: TokenModel;

  constructor(
    private authService: LoginService,
    private router: Router,
    private basket: BasketService,

  ) { }

  ngOnInit() {
  }

  validPassEmail() {
    this.passOrUser = false;
  }

  loginUser(login: NgForm) {
    const dto: LoginViewModel = {
      email: this.emailLogin,
      password: this.passwordLogin,
      rememberMe: true,
    };

    if (login.valid) {
      this.isLoading = true;
      this.passOrUser = false;
      this.authService.loginUser(dto).pipe(
        tap(resp => {
          this.token = ({
            accessToken: resp.accessToken,
            refreshToken: resp.refreshToken
          });

        }),
        catchError((err: HttpErrorResponse) => {
          if (err.status === 400) {
            this.passOrUser = true;
            return throwError('User email or password is incorrect!');
          }
          return throwError(err);

        }
        ),
        filter(() => !!localStorage.getItem('basketId')),
        switchMap(resp =>
          this.basket.synchronizeBaskets(+localStorage.getItem('basketId'), +this.authService.jwtToken.BasketId).pipe(
            tap(resp => localStorage.setItem('basketId', JSON.stringify(resp)))
          )),
        finalize(() => {
          this.isLoading = false;
          this.router.navigate(['/purchase'])
        })
      ).subscribe();
    }
  }
}
