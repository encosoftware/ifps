import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SupplyRoutingModule } from './supply-routing.module';
import { SharedModule } from '../shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { supplyOrdersReducers } from './supply-orders/store/reducers';
import { cargoListReducers } from './cargo/store/reducers';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    SupplyRoutingModule,
    SharedModule,
    StoreModule.forFeature('supplyOrders', supplyOrdersReducers),
    StoreModule.forFeature('cargos', cargoListReducers)
  ]
})
export class SupplyModule { }
