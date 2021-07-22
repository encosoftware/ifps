import { NgModule } from '@angular/core';

import { PurchaseRoutingModule } from './purchase-routing.module';
import { PurchaseComponent } from './pages/purchase.component';
import { SharedModule } from '../shared/shared.module';
import { PurchaseFinishComponent } from './pages/purchase-finish/purchase-finish.component';

@NgModule({
  declarations: [PurchaseComponent, PurchaseFinishComponent],
  imports: [
    SharedModule,
    PurchaseRoutingModule
  ]
})
export class PurchaseModule { }
