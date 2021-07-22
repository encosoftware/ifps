import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { TrendsRoutingModule } from './trends-routing.modulte';
import { TrendsComponent } from './pages/trends/trends.component';
import { StoreModule } from '@ngrx/store';
import { trendsReducers } from './store/reducers';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [TrendsComponent],
  imports: [
    CommonModule,
    SharedModule,
    TrendsRoutingModule,
    FormsModule,
    StoreModule.forFeature('trends', trendsReducers)
  ]
})
export class TrendsModule { }
