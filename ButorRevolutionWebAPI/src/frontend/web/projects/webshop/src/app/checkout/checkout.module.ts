import { NgModule } from '@angular/core';

import { CheckoutRoutingModule } from './checkout-routing.module';
import { CheckoutComponent } from './pages/checkout.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [CheckoutComponent],
  imports: [
    SharedModule,
    CheckoutRoutingModule
  ]
})
export class CheckoutModule { }
