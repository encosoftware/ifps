import { Component, OnInit } from "@angular/core";
import { LoginService } from '../../../login/services/login.service';
import { TokenModel } from '../../../shared/models/auth';
import { map, finalize } from 'rxjs/operators';

@Component({
  selector: "app-footer",
  templateUrl: "./footer.component.html",
  styleUrls: ["./footer.component.scss"]
})
export class FooterComponent implements OnInit {
  loggedIn: TokenModel;
  constructor(
    private loginService: LoginService
  ) { }

  ngOnInit() {
    this.loginService.logedInSubject.pipe(
      map(login => this.loggedIn = login)
    ).subscribe();
  }
  logout() {
    this.loginService.logoutUser(this.loggedIn.accessToken).subscribe();
  }
}
