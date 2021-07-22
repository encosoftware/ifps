import { Component, OnInit, Input } from '@angular/core';
import { IOrderHistoryListViewModel } from '../../models/orders';

@Component({
  selector: 'butor-order-history',
  templateUrl: './order-history.component.html',
  styleUrls: ['./order-history.component.scss']
})
export class OrderHistoryComponent implements OnInit {

  @Input() history: IOrderHistoryListViewModel;

  constructor(
  ) { }

  ngOnInit(): void {
  }

}
