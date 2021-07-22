import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductsComponent } from './pages/products.component';
import { EditProductsComponent } from './pages/edit-products/edit-products.component';
import { CanDeactivateGuard } from '../../core/guards/can-deactivate.guard';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
    path: '',
    component: ProductsComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum['GetFurnitureUnits']]
    },
  },
  {
    path: ':id',
    component: EditProductsComponent,
    canDeactivate: [CanDeactivateGuard],
    data: {
      breadcrumb: 'Edit Product',
      claims: ClaimPolicyEnum[ClaimPolicyEnum['UpdateFurnitureUnits']]
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductsRoutingModule { }
