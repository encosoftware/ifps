import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { UsersService } from '../../services/users.service';
import { tap, debounceTime, distinctUntilChanged, finalize, catchError } from 'rxjs/operators';
import { SnackbarService } from 'butor-shared-lib';
import { of } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IUsersCreateListViewModel } from '../../models/users.models';

@Component({
  selector: 'butor-add-new-user',
  templateUrl: './add-new-user.component.html'
})
export class AddNewUserComponent implements OnInit {
  roles = [];
  emailExist = false;
  userAddForm: FormGroup;
  constructor(
    public dialogRef: MatDialogRef<any>,
    private router: Router,
    private userService: UsersService,
    private snackBar: SnackbarService,
    private formBuilder: FormBuilder
  ) {
  }

  ngOnInit() {
    this.userAddForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      roleId: [null, [Validators.required]],
      phoneNumber: ['', Validators.pattern('((?:\\+?3|0)6)(?:-|\\()?(\\d{1,2})(?:-|\\))?(\\d{3})-?(\\d{3,4})')],
      email: ['', [Validators.email, Validators.required]]
    });
    this.userAddForm.controls['email'].valueChanges.pipe(
      debounceTime(700),
      distinctUntilChanged((x, y) => x === y)).subscribe(res => {
        this.emailValidation(res);
      });
    this.userService.getRoles().pipe(
      tap(resp => this.roles = resp)
    ).subscribe();
  }

  cancel(): void {
    this.dialogRef.close();
  }

  addNewUser(value: IUsersCreateListViewModel) {
    this.userService.newUser(value).pipe(
      tap(resp => this.router.navigate([`/admin/users/`, resp])),
      catchError(err => of(this.snackBar.customMessage(err))),
      finalize(() => { this.dialogRef.close(); })
    ).subscribe();
  }

  emailValidation(email) {
    this.emailExist = false;
    if (email) {
      this.userService.emailValidation(email).pipe(
        tap(e => this.emailExist = e),
      ).subscribe();
    } else {
      return;
    }
  }
}
