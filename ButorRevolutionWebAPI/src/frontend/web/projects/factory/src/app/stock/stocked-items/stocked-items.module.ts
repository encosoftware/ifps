import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { StockedItemRoutingModule } from './stocked-items-routing.module';
import { StockedItemsPageComponent } from './pages/stocked-items.page.component';
import { StockedItemsService } from './services/stocked-items.service';
import { StoreModule } from '@ngrx/store';
import { stockeditemReducers } from './store/reducers';
import { NewStockedItemComponent } from './components/new-stocked-item/new-stocked-item.component';
import { ReserveStockedItemsPageComponent } from './pages/reserve-stocked-items/reserve-stocked-items.page.component';
import { EjectStockedItemsPageComponent } from './pages/eject-stocked-items/eject-stocked-items.page.component';


@NgModule({
  declarations: [
    StockedItemsPageComponent,
    NewStockedItemComponent,
    ReserveStockedItemsPageComponent,
    EjectStockedItemsPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    StockedItemRoutingModule,
    StoreModule.forFeature('stockedItems', stockeditemReducers)
  ],
  entryComponents: [
    NewStockedItemComponent
  ],
  providers: [
    StockedItemsService
  ]
})
export class StockedItemsModule { }
