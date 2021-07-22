import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductsDetailsComponent } from './pages/products-details.component';

const routes: Routes = [
    {
      path: ':id',
      component: ProductsDetailsComponent
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductsDetailsRoutingModule { }
