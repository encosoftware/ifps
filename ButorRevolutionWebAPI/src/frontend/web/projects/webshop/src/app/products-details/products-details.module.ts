import { NgModule } from '@angular/core';

import { ProductsDetailsRoutingModule } from './products-details-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ProductsDetailsComponent } from './pages/products-details.component';

@NgModule({
  declarations: [ProductsDetailsComponent],
  imports: [
    SharedModule,
    ProductsDetailsRoutingModule
  ]
})
export class ProductsDetailsModule { }
