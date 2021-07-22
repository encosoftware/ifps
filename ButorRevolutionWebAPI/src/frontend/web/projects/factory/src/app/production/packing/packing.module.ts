import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { PackingRoutingModule } from './packing-routing.module';
import { PackingPageComponent } from './pages/packing.component';
import { StoreModule } from '@ngrx/store';
import { packingReducers } from './store/reducers';

@NgModule({
  declarations: [
    PackingPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    PackingRoutingModule,
    StoreModule.forFeature('packing', packingReducers)
  ]
})
export class PackingModule { }
