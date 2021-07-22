import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrdersPageComponent } from './pages/orders.page.component';
import { EditOrderPageComponent } from './pages/edit-order/edit-order.page.component';
import { OfferFormPageComponent } from './pages/offer-form/offer-form.page.component';
import { ContractFormPageComponent } from './pages/contract-form/contract-form.page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
    path: '',
    component: OrdersPageComponent,
    // data: {
    //   claims: [ClaimPolicyEnum[ClaimPolicyEnum.GetOrders],ClaimPolicyEnum[ClaimPolicyEnum.GetOrdersBySales],ClaimPolicyEnum[ClaimPolicyEnum.GetOrdersByCustomer]]
    // }
  },
  {
    path: ':id/offerform',
    component: OfferFormPageComponent,

  },
  {
    path: ':id/contractform',
    component: ContractFormPageComponent
  },
  {
    path: ':id',
    component: EditOrderPageComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrdersRoutingModule { }
