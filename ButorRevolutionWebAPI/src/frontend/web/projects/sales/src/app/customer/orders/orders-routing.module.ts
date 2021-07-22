import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ViewOrderComponent } from './pages/view-order/view-order.component';

const routes: Routes = [
  {
    path: ''
  },
  {
    path: ':id',
    component: ViewOrderComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrdersRoutingModule { }
