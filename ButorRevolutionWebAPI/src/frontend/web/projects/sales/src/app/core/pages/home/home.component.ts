import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'butor-sales-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent  {
  // emailLogin: string;
  // passwordLogin: string;
  // isLoading: boolean;
  // passOrUser: boolean;
  // authService: any;

  // constructor() { }

  // ngOnInit() {
  // }

  // loginUser(login: NgForm) {
  //   const dto: LoginViewModel = {
  //     email: this.emailLogin,
  //     password: this.passwordLogin,
  //     rememberMe: true,
  //   };

  //   if (login.valid) {
  //     this.isLoading = true;
  //     this.passOrUser = false;
  //     this.authService.loginUser(dto).pipe(
  //       tap(resp => {
  //         this.token = ({
  //           accessToken: resp.accessToken,
  //           refreshToken: resp.refreshToken
  //         });

  //       }),
  //       // finalize(() => this.isLoading = false),
  //       catchError((err: HttpErrorResponse) => {
  //         if (err.status === 400) {
  //           this.passOrUser = true;
  //           return throwError('User email or password is incorrect!');
  //         }
  //         return throwError(err);

  //       }
  //       )
  //     ).subscribe(
  //       resp => {
  //         this.isLoading = false;
  //         this.router.navigate(['sales/dashboard']);
  //       }
  //     );
  //   }
  // }
  // validPassEmail() {
  //   this.passOrUser = false;
  // }
  // registerUser(register: NgForm) {
  //   if (register.valid) {
  //     if ((this.password === this.passwordcheck)) {
  //       this.isLoading = true;
  //       this.authService.registrationUser(this.name, this.email, this.password,false).subscribe(
  //         resp => this.router.navigate(['sales/dashboard']),
  //         () => { },
  //         () => this.isLoading = false
  //       );
  //     }

  //   }
  // }

  // emailValidation(email) {
  //   if (email) {
  //     this.authService.emailValidation(email).pipe(
  //       debounceTime(500),
  //       distinctUntilChanged((x, y) => x === y),
  //       tap(e => this.emailExist = e),
  //     ).subscribe();
  //   } else {
  //     return;
  //   }
  // }

  // openDialog(id?: number, token?: string): void {
  //   const dialogRef = this.dialog.open(ForgotPasswordComponent, {
  //     width: '48rem',
  //     data: {
  //       userId: id,
  //       token
  //     }
  //   });

  //   // dialogRef.afterClosed().subscribe(result => {

  //   // });
  // }

}
