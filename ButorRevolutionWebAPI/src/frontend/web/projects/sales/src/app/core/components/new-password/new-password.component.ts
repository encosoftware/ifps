import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'butor-new-password',
  templateUrl: './new-password.component.html',
  styleUrls: ['./new-password.component.scss']
})
export class NewPasswordComponent implements OnInit {

  passwordForm: FormGroup;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<any>,
  ) { }

  ngOnInit() {
    this.passwordForm = this.formBuilder.group({
      currentPassword: ['', Validators.required],
      // tslint:disable-next-line: max-line-length
      newPassword1: ['', [Validators.pattern('^(?:(?=.*[a-z])(?:(?=.*[A-Z])(?=.*[\\d\\W])|(?=.*\\W)(?=.*\\d))|(?=.*\\W)(?=.*[A-Z])(?=.*\\d)).{6,}$'), Validators.required]],
      // tslint:disable-next-line: max-line-length
      newPassword2: ['', [Validators.pattern('^(?:(?=.*[a-z])(?:(?=.*[A-Z])(?=.*[\\d\\W])|(?=.*\\W)(?=.*\\d))|(?=.*\\W)(?=.*[A-Z])(?=.*\\d)).{6,}$'), Validators.required]],
    });
  }

  get f() { return this.passwordForm.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.passwordForm.invalid || (this.passwordForm.controls.newPassword1.value !== this.passwordForm.controls.newPassword2.value)) {
      return;
    }

    this.save();
  }

  save(): void {
    const password = {
      currentPassword: this.passwordForm.controls.currentPassword.value,
      newPassword: this.passwordForm.controls.newPassword1.value
    };
    this.dialogRef.close(password);
  }

  cancel(): void {
    this.dialogRef.close();
  }

}
