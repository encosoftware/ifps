import { NgModule } from '@angular/core';

import { AccountHistoryRoutingModule } from './account-history-routing.module';
import { AccountHistoryComponent } from './pages/account-history.component';
import { SharedModule } from '../shared/shared.module';
import { AccountComponent } from './components/account/account.component';
import { OrdersComponent } from './components/orders/orders.component';
import { OrderViewComponent } from './components/order-view/order-view.component';

@NgModule({
  declarations: [AccountHistoryComponent, AccountComponent, OrdersComponent, OrderViewComponent],
  imports: [
    SharedModule,
    AccountHistoryRoutingModule
  ],
  entryComponents: [OrderViewComponent],

})
export class AccountHistoryModule { }
