import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { tap, catchError, debounceTime, distinctUntilChanged, map, finalize } from 'rxjs/operators';
import { LoginViewModel, TokenModel } from '../../models/auth';
import { throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ForgotPasswordComponent } from '../forgot-password/forgot-password.component';
import { SnackbarService } from 'butor-shared-lib';
import { TranslateService } from '@ngx-translate/core';

@Component({
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
    private authService: AuthService,
    private router: Router,
    private activRoute: ActivatedRoute,
    private dialog: MatDialog,
    private snackBar: SnackbarService,
    private translate: TranslateService
  ) {
    if (
      !!this.activRoute.snapshot.queryParams &&
      !!this.activRoute.snapshot.queryParams.dialog === true &&
      !!this.activRoute.snapshot.queryParams.passwordresettoken &&
      !!this.activRoute.snapshot.queryParams.userId) {
      this.openDialog(+this.activRoute.snapshot.queryParams.userId, this.activRoute.snapshot.queryParams.passwordresettoken);

    } else if (
      !!this.activRoute.snapshot.queryParams &&
      !!this.activRoute.snapshot.queryParams.token &&
      !!this.activRoute.snapshot.queryParams.userId) {
      this.authService.confirmedEmail(+this.activRoute.snapshot.queryParams.userId, this.activRoute.snapshot.queryParams.token).subscribe(
        resp => this.snackBar.customMessage(this.translate.instant('snackbar.success'))
      );
    }
  }

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
            throwError('User email or password is incorrect!');
          }
          return this.router.navigate(['']);
        }),
        finalize(() => this.isLoading = false),
      ).subscribe(
        resp => {
          this.isLoading = false;
          this.router.navigate(['']);
        }
      );
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
          resp => {
            this.router.navigate(['']);
            this.snackBar.customMessage(this.translate.instant('email.success')
            )
          },
          (err) => {this.snackBar.customMessage(this.translate.instant('snackbar.error')); this.isLoading = false },
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

  openDialog(id?: number, passwordresettoken?: string): void {
    const dialogRef = this.dialog.open(ForgotPasswordComponent, {
      width: '48rem',
      data: {
        userId: id,
        passwordresettoken
      }
    });

  }

}
