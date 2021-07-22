import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { StatisticsRoutingModule } from './statistics-routing.module';
import { StatisticsPageComponent } from './statistics-page/statistics-page.component';

@NgModule({
  declarations: [
    StatisticsPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    StatisticsRoutingModule,
  ]
})
export class StockStatisticsModule { }
