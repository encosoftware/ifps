import { Component, OnInit } from '@angular/core';
import { TokenModel, LoginViewModel } from '../../shared/models/auth';
import { LoginService } from '../services/login.service';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { tap, catchError, debounceTime, distinctUntilChanged, switchMap, filter, finalize, takeWhile } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { BasketService } from '../../basket/services/basket.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  name: string;
  email: string;
  emailLogin = 'enco@enco.hu';
  password: string;
  passwordLogin = 'password';
  passwordcheck: string;
  token: TokenModel;
  passOrUser = false;
  emailExist = false;
  isLoading = false;

  constructor(
    private authService: LoginService,
    private basket: BasketService,
    private router: Router,
  ) { }

  ngOnInit() {
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
            tap(resp => {
              if (!(resp === 0)) {
                localStorage.setItem('basketId', JSON.stringify(resp))
              }
            })
          )),
        finalize(() => {
          this.isLoading = false;
          this.router.navigate(['/'])
        })
      ).subscribe();
    }
  }
  validPassEmail() {
    this.passOrUser = false;
  }
  registerUser(register: NgForm) {
    if (register.valid) {
      if ((this.password === this.passwordcheck)) {
        this.isLoading = true;
        this.authService.registrationUser(this.name, this.email, this.password, false).subscribe(
          resp => this.router.navigate(['']),
          () => { },
          () => this.isLoading = false
        );
      }

    }
  }

  emailValidation(email) {
    if (email) {
      this.authService.emailValidation(email).pipe(
        debounceTime(500),
        distinctUntilChanged((x, y) => x === y),
        tap(e => this.emailExist = e),
      ).subscribe();
    } else {
      return;
    }
  }


}
