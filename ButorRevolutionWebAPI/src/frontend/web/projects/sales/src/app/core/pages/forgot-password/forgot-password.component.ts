import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AuthService } from '../../services/auth.service';
import { SnackbarService } from 'butor-shared-lib';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'butor-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  email: string;
  password: string;
  passwordcheck: string;
  forgotPassword = true;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private authService: AuthService,
    private dialogRef: MatDialogRef<any>,
    private snackBar: SnackbarService,
    public translate: TranslateService
  ) { }

  ngOnInit() {
    this.email = '';
    this.password = '';
    this.passwordcheck = '';
    if (this.data && this.data.userId && this.data.passwordresettoken) {
      this.forgotPassword = false;
    }
  }

  changePassword() {
    if (this.forgotPassword) {
      this.authService.resetPassword(this.email).subscribe(
        resp => {
          this.dialogRef.close();
          this.snackBar.customMessage(this.translate.instant('snackbar.success'));
        }
      );

    } else if (!this.forgotPassword) {
      this.authService.changePassword(this.data.userId, this.data.passwordresettoken, this.password).subscribe(
        resp => {
          this.dialogRef.close();
          this.snackBar.customMessage(this.translate.instant('snackbar.success'));
        }
      );
    }
  }
}
