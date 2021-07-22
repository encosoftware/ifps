import { Component, OnInit } from '@angular/core';
import { OrdersService } from '../services/orders.service';
import { map, take, switchMap } from 'rxjs/operators';
import { WebshopOrdersListViewModel } from '../models/orders';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../login/store/selectors/core.selector';

@Component({
  selector: 'app-account-history',
  templateUrl: './account-history.component.html',
  styleUrls: ['./account-history.component.scss']
})
export class AccountHistoryComponent implements OnInit {
  orders: WebshopOrdersListViewModel[] = [];

  constructor(
    private orderService: OrdersService,
    private store: Store<any>,
  ) { }

  ngOnInit() {
    this.store.pipe(
      select(coreLoginT),
      take(1),
      map(res =>  +res.UserId),
      switchMap((id) =>
      this.orderService.getOrdersByCustomerIdByWebshop(id).pipe(
        map(resp => this.orders = resp),
      )
      )
    )
    .subscribe();
  }


}
