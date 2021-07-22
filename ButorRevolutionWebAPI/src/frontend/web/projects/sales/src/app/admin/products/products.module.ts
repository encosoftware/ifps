import { NgModule } from '@angular/core';

import { ProductsRoutingModule } from './products-routing.module';
import { ProductsComponent } from './pages/products.component';
import { SharedModule } from '../../shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { productsReducers } from './store/reducers';
import { EditProductsComponent } from './pages/edit-products/edit-products.component';
import { FrontComponent } from './components/front/front.component';
import { CorpusComponent } from './components/corpus/corpus.component';
import { AccessoryComponent } from './components/accessory/accessory.component';
import { NewProductUnitComponent } from './components/new-product-unit/new-product-unit.component';

@NgModule({
  declarations: [
    ProductsComponent,
    EditProductsComponent,
    FrontComponent,
    CorpusComponent,
    AccessoryComponent,
    NewProductUnitComponent
  ],
  imports: [
    ProductsRoutingModule,
    SharedModule,
    StoreModule.forFeature('products', productsReducers)
  ],
  entryComponents: [
    FrontComponent,
    CorpusComponent,
    AccessoryComponent,
    NewProductUnitComponent
  ],
})
export class ProductsModule { }
