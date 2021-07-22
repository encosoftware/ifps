import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { LoginService } from '../../login/services/login.service';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  name: string;
  email: string;
  password: string;
  passwordcheck: string;
  emailExist = false;
  isLoading = false;
  emailSend = false;
  emailTermsC = false;
  constructor(
    private router: Router,
    private authService: LoginService,

  ) { }

  ngOnInit() {
  }

  registerUser(register: NgForm) {
    if (register.valid) {
      if ((this.password === this.passwordcheck)) {
        this.isLoading = true;
        this.authService.registrationUser(this.name, this.email, this.password, this.emailSend).subscribe(
          resp => this.router.navigate(['']),
          () => { },
          () => this.isLoading = false
        );
      }

    }
  }

  navigateLogin() {
    this.router.navigate(['/login']);
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
