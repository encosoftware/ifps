import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from '../../shared/shared.module';
import { OrderSchedulingRoutingModule } from './order-scheduling-routing';
import { OrderSchedulingPageComponent } from './pages/order-scheduling.page.component';
import { orderSchedulingsReducers } from './store/reducers';
import { UnderProductionComponent } from './components/under-production/under-production.component';
import { OptimisationModalComponent } from './components/optimisation-modal/optimisation-modal.component';
import { PrintCodesComponent } from './components/print-codes/print-codes.component';
import { EvenOddPipe } from '../../shared/pipes/evenodd.pipe';

@NgModule({
  declarations: [
    OrderSchedulingPageComponent,
    UnderProductionComponent,
    OptimisationModalComponent,
    PrintCodesComponent,
    EvenOddPipe
  ],
  imports: [
    CommonModule,
    SharedModule,
    OrderSchedulingRoutingModule,
    StoreModule.forFeature('orderSchedulings', orderSchedulingsReducers)
  ],
  entryComponents: [
    OptimisationModalComponent,
    PrintCodesComponent
  ],
  providers: [
  ]
})
export class OrderSchedulingModule { }
