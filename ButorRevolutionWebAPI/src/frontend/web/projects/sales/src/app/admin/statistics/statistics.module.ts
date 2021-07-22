import { NgModule } from '@angular/core';

import { StatisticsRoutingModule } from './statistics-routing.module';
import { StatisticsComponent } from './pages/statistics/statistics.component';
import { SharedModule } from '../../shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { statisticsReducers } from './store/reducers';
import { StatisticsHeaderComponent } from './components/statistics-header/statistics-header.component';

@NgModule({
  declarations: [StatisticsComponent, StatisticsHeaderComponent],
  imports: [
    SharedModule,
    StatisticsRoutingModule,
    StoreModule.forFeature('statistics', statisticsReducers)
  ]
})
export class StatisticsModule { }
