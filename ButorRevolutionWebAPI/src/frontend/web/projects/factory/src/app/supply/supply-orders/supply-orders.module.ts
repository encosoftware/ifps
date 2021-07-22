import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SupplyOrdersComponent } from './pages/supply-orders.component';
import { SupplyOrdersRoutingModule } from './supply-orders-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { SupplyCheckboxPipe } from '../checkbox.pipe';
import { CreateCargoComponent } from './pages/create-cargo/create-cargo.component';
import { SupplyMaterialsComponent } from './components/supply-materials/supply-materials.component';
import { OrderAdditionalsComponent } from './components/order-additionals/order-additionals.component';
import { OrderShippingComponent } from './components/order-shipping/order-shipping.component';
import { CargoPreviewComponent } from './components/cargo-preview/cargo-preview.component';
import { SupplierCheckBoxPipe } from './pipes/supplier-check-box.pipe';

@NgModule({
  declarations: [
    SupplyOrdersComponent,
    SupplyCheckboxPipe,
    CreateCargoComponent,
    SupplyMaterialsComponent,
    OrderAdditionalsComponent,
    OrderShippingComponent,
    CargoPreviewComponent,
    SupplierCheckBoxPipe
  ],
  imports: [
    CommonModule,
    SupplyOrdersRoutingModule,
    SharedModule
  ],
  entryComponents: [
    CargoPreviewComponent
  ]
})
export class SupplyOrdersModule { }
