import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SortingPageComponent } from './pages/sorting.component';
import { SharedModule } from '../../shared/shared.module';
import { SortingRoutingModule } from './sorting-routing.module';
import { sortingsReducers } from './store/reducers';
import { StoreModule } from '@ngrx/store';

@NgModule({
  declarations: [
    SortingPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    SortingRoutingModule,
    StoreModule.forFeature('sorting', sortingsReducers)
  ]
})
export class SortingModule { }
