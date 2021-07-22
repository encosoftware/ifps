import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { UsersService } from '../../services/users.service';
import { IUsersCreateListViewModel } from '../../models/users.models';
import { tap, debounceTime, distinctUntilChanged, finalize } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'butor-add-new-user',
  templateUrl: './add-new-user.component.html'
})
export class AddNewUserComponent implements OnInit {
  roles = [];
  emailExist = false;
  constructor(
    public dialogRef: MatDialogRef<any>,
    private router: Router,
    private userService: UsersService) {
  }

  ngOnInit() {
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
      finalize(() => { this.dialogRef.close(); })
    ).subscribe();
  }

  emailValidation(email) {
    if (email) {
       this.userService.emailValidation(email).pipe(
        debounceTime(500),
        distinctUntilChanged((x, y) => x === y),
        tap(e => this.emailExist = e),
      ).subscribe();
    } else {
      return;
    }
  }
}
