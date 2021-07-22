import { NgModule } from '@angular/core';

import { SalesRoutingModule } from './sales-routing.module';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [],
  imports: [
    SharedModule,
    SalesRoutingModule,
  ]
})
export class SalesModule { }
