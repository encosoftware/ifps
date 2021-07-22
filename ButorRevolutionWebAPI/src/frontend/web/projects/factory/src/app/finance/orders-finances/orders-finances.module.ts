import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { OrdersFinancesRoutingModule } from './orders-finances-routing.module';
import { OrdersFinancesComponent } from './pages/orders-finances.component';
import { StoreModule } from '@ngrx/store';
import { ordersCustomerReducers } from './store/reducers';
import { OrdersViewComponent } from './pages/orders-view/orders-view.component';
import { OrderHeaderComponent } from './components/order-header/order-header.component';

@NgModule({
  declarations: [OrdersFinancesComponent, OrdersViewComponent, OrderHeaderComponent],
  imports: [
    CommonModule,
    SharedModule,
    OrdersFinancesRoutingModule,
    StoreModule.forFeature('ordersFinances', ordersCustomerReducers )
  ]
})
export class OrdersFinancesModule { }
