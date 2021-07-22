import { NgModule } from '@angular/core';

import { AdminRoutingModule } from './admin-routing.module';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [
    AdminRoutingModule,
    SharedModule
  ],
})
export class AdminModule { }
