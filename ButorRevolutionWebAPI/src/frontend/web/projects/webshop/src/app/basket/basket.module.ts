import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BasketRoutingModule } from './basket-routing.module';
import { BasketsComponent } from './pages/baskets.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [BasketsComponent],
  imports: [
    SharedModule,
    BasketRoutingModule
  ]
})
export class BasketModule { }
