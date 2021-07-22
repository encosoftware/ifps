import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PurchaseComponent } from './pages/purchase.component';
import { PurchaseFinishComponent } from './pages/purchase-finish/purchase-finish.component';
import { BasketEmptyGuard } from '../core/guards/basket-empty.guard';

const routes: Routes = [
  {
    path: '',
     canActivate: [BasketEmptyGuard],
    canActivateChild: [BasketEmptyGuard],
    component: PurchaseComponent
  },
  {
    path: 'success',
    component: PurchaseFinishComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PurchaseRoutingModule { }
