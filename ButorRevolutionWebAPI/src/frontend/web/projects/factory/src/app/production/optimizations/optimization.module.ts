import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from '../../shared/shared.module';
import { optimizationsReducers } from './store/reducers';
import { OptimizationRoutingModule } from './optimization-routing';
import { OptimizationPageComponent } from './pages/optimization-page.component';

@NgModule({
  declarations: [
    OptimizationPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    OptimizationRoutingModule,
    StoreModule.forFeature('optimizations', optimizationsReducers)
  ],
  entryComponents: [
  ],
  providers: [
  ]
})
export class OptimizationModule { }
