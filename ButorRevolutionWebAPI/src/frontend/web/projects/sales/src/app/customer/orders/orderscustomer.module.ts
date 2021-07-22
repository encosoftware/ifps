import { NgModule } from '@angular/core';

import { OrdersRoutingModule } from './orders-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { ordersCustomerReducers } from './store/reducers';
import { ViewOrderComponent } from './pages/view-order/view-order.component';
import { CustomerOrderDocumentsComponent } from './components/customer-order-documents/customer-order-documents.component';
import { OrderHeaderComponent } from './components/costumer-order-header/order-header.component';
import { CustomerOrderAppointmentsComponent } from './components/customer-order-appointments/customer-order-appointments.component';
import { CustomerOrderMessagesComponent } from './components/customer-order-messages/customer-order-messages.component';
import { CustomerOrderHistoryComponent } from './components/customer-order-history/customer-order-history.component';
import { CustomerOfferComponent } from './pages/customer-offer/customer-offer.component';
import { CustomerContractComponent } from './pages/customer-contract/customer-contract.component';

@NgModule({
  declarations: [
    ViewOrderComponent,
    OrderHeaderComponent,
    CustomerOrderDocumentsComponent,
    CustomerOrderAppointmentsComponent,
    CustomerOrderMessagesComponent,
    CustomerOrderHistoryComponent,
    CustomerOfferComponent,
    CustomerContractComponent
  ],
  imports: [
    SharedModule,
    OrdersRoutingModule,
    StoreModule.forFeature('ordersCustomer', ordersCustomerReducers)

  ]
})
export class OrdersCustomerModule { }
