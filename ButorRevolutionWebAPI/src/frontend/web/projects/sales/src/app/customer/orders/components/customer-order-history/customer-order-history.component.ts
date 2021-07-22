import { Component, OnInit, Input } from '@angular/core';
import { IOrderTicketModel } from '../../../../sales/orders/models/ticket.model';

@Component({
  selector: 'butor-customer-order-history',
  templateUrl: './customer-order-history.component.html',
  styleUrls: ['./customer-order-history.component.scss']
})
export class CustomerOrderHistoryComponent implements OnInit {

  @Input() history;

  constructor() { }

  ngOnInit() {
  }

}
