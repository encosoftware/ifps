import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog.component';

@Injectable({
  providedIn: 'root',
})
export class DialogService {

  constructor(
    public dialog: MatDialog
  ) {}

  confirm(message?: string): Observable<boolean> {
   
    // let dialogRef = this.dialog.open(ConfirmDialogComponent, {
    //   width: '400px',
    //   data: message
    // });
    // dialogRef.afterClosed().subscribe(result => {
    //   return of(result);
    // });

    const confirmation = window.confirm(message);
 
    return of(confirmation);

  }
}
