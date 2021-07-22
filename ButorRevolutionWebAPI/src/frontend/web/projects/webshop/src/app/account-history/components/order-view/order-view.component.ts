import { Component, OnInit, Inject } from '@angular/core';
import { ShippingServiceListViewModel } from '../../../basket/models/basket';
import { TokenModel } from '../../../shared/models/auth';
import { Subscription } from 'rxjs';
import { map, finalize } from 'rxjs/operators';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { WebshopOrdersListViewModel, WebshopOrdersDetailsViewModel } from '../../models/orders';
import { OrdersService } from '../../services/orders.service';

@Component({
  selector: 'webshop-order-view',
  templateUrl: './order-view.component.html',
  styleUrls: ['./order-view.component.scss']
})
export class OrderViewComponent implements OnInit {

  viewOrderDetails: WebshopOrdersDetailsViewModel;
  isLoading = false;

  constructor(
    private ordersService: OrdersService,
    @Inject(MAT_DIALOG_DATA) public data: WebshopOrdersListViewModel,


  ) { }

  ngOnInit() {
    this.ordersService.getOrderedFurnitureUnitsDetails(this.data.id).pipe(
      map(res => this.viewOrderDetails = res),
      finalize(() => this.isLoading = true)
    ).subscribe();
  }


}
