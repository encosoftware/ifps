import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from '../../shared/shared.module';
import { CargoPageComponent } from './pages/cargo.page.component';
import { StorageCargoRoutingModule } from './cargo-routing.module';
import { CargoService } from './services/cargo.service';
import { cargoReducers } from './store/reducers';
import { CargoHeaderComponent } from './components/cargo-header/cargo-header.component';
import { CargoDetailsComponent } from './components/cargo-details/cargo-details.component';
import { StockCargoPageComponent } from './pages/edit-cargo/edit-cargo.page.component';

@NgModule({
  declarations: [
    CargoPageComponent,
    StockCargoPageComponent,
    CargoHeaderComponent,
    CargoDetailsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    StorageCargoRoutingModule,
    StoreModule.forFeature('cargo', cargoReducers)
  ],
  entryComponents: [
  ],
  providers: [
    CargoService
  ]
})
export class StorageCargo { }
