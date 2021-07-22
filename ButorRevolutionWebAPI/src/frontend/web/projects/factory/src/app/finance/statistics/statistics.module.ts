import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { StatisticsRoutingModule } from './statistics-routing.module';
import { statisticsReducers } from './store/reducers';
import { StatisticsPageComponent } from './pages/statistics-page.component';

@NgModule({
  declarations: [
    StatisticsPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    StatisticsRoutingModule,
    StoreModule.forFeature('statistics', statisticsReducers)
  ],
  entryComponents: [
  ]
})
export class FinanceStatisticsModule { }
