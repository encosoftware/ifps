import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CargoComponent } from './pages/cargo.component';
import { CargoRoutingModule } from './cargo-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { ArrivedCargoComponent } from './pages/arrived-cargo/arrived-cargo.component';
import { ConfirmationCargoComponent } from './pages/confirmation-cargo/confirmation-cargo.component';

@NgModule({
  declarations: [
    CargoComponent,
    ConfirmationCargoComponent,
    ArrivedCargoComponent
  ],
  imports: [
    CommonModule,
    CargoRoutingModule,
    SharedModule
  ]
})
export class CargoModule { }
