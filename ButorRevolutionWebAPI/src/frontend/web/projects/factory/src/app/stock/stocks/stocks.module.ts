import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoragesComponent } from './pages/stocks.component';
import { StocksRoutingModule } from './stocks-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { StoragesService } from './services/stocks.service';
import { StoreModule } from '@ngrx/store';
import { stockReducers } from './store/reducers';
import { NewStorageComponent } from './components/new-stock/new-stock.component';

@NgModule({
  declarations: [
    StoragesComponent,
    NewStorageComponent
  ],
  imports: [
    CommonModule,
    StocksRoutingModule,
    SharedModule,
    StoreModule.forFeature('stocks', stockReducers)
  ],
  entryComponents: [
    NewStorageComponent
  ],
  providers: [StoragesService]
})
export class StocksModule { }
