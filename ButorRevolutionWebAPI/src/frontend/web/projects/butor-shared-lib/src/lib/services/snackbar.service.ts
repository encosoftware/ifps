import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackbarService {

  constructor(
    public snackBar: MatSnackBar
  ) { }

  customMessage(message: string): void {
    this.snackBar.open(message, null, {
      duration: 3000,
      horizontalPosition: 'right'
    });
  }

  error(): void {
    this.snackBar.open('Unexpected error!', 'Close', {
      duration: 3000
    });
  }

  saved(): void {
    this.snackBar.open('SAVED', null, {
      duration: 3000,
      horizontalPosition: 'right'
    });
  }

  success(): void {
    this.snackBar.open('SUCCESS', 'Cancel', {
      duration: 3000,
      horizontalPosition: 'right'
    });
  }

  deleted(): void {
    this.snackBar.open('DELETED', null, {
      duration: 3000,
      horizontalPosition: 'right'
    });
  }
}
