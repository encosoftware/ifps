import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SnackbarService } from 'butor-shared-lib';
import { IOrderAppointmentListViewModel } from '../../models/orders';
import { OrdersService } from '../../services/orders.service';
import { OrderNewAppointmentComponent } from '../new-appointment/order-new-appointment.component';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { setMinutes, setHours } from 'date-fns';

@Component({
  selector: 'butor-order-appointments',
  templateUrl: './order-appointments.component.html',
  styleUrls: ['./order-appointments.component.scss']
})
export class OrderAppointmentsComponent implements OnInit {

  @Input() appointments: IOrderAppointmentListViewModel[];
  @Input() orderIdApp: string;
  @Output() saved = new EventEmitter();

  constructor(
    public dialog: MatDialog,
    private orderService: OrdersService,
    private snackBar: SnackbarService,
    private route: ActivatedRoute,
    private translate: TranslateService
  ) { }

  ngOnInit(): void {
  }

  editAppointment(id: number): void {
    this.orderService.getAppointment(id).subscribe(res => {
      const dialogRef = this.dialog.open(OrderNewAppointmentComponent, {
        width: '98rem',
        data: {
          data: res,
          orderid: this.orderIdApp
        }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result !== undefined) {
          this.orderService.putAppointment(id, result).subscribe(() => {
            this.snackBar.customMessage(this.translate.instant('snackbar.saved'));
            this.saved.emit();
          });
        }
      });
    });
  }

  addAppointment(): void {
    const dialogRef = this.dialog.open(OrderNewAppointmentComponent, {
      width: '98rem',
      data: {
        date: setMinutes(setHours(new Date(), 8), 0),
        orderid: this.orderIdApp
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.orderService.postAppointment(this.route.snapshot.paramMap.get('id'), result).subscribe(res => {
          this.snackBar.customMessage(this.translate.instant('snackbar.success'));
          this.saved.emit();
        },
        (err) => this.snackBar.customMessage(this.translate.instant('snackbar.wrongStatus')));
      }
    });

  }

  deleteAppointment(id: number) {
    this.orderService.deleteAppointment(id).subscribe(() => {
      this.snackBar.customMessage(this.translate.instant('snackbar.deleted'));
      this.saved.emit();
    });
  }

}
