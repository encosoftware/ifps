import { Component, OnInit, Input } from '@angular/core';
import { IOrderAppointmentListViewModel } from '../../../../sales/orders/models/orders';

@Component({
  selector: 'butor-customer-order-appointments',
  templateUrl: './customer-order-appointments.component.html',
  styleUrls: ['./customer-order-appointments.component.scss']
})
export class CustomerOrderAppointmentsComponent implements OnInit {
  @Input() appointments: IOrderAppointmentListViewModel[];

  constructor(
  ) { }

  ngOnInit(): void {

  }

}
