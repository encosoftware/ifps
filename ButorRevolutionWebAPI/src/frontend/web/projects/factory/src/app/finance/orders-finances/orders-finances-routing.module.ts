import { Routes, RouterModule } from '@angular/router';

import { NgModule } from '@angular/core';
import { OrdersFinancesComponent } from './pages/orders-finances.component';
import { OrdersViewComponent } from './pages/orders-view/orders-view.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: OrdersFinancesComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetOrderExpenses]
          },
    },
    {
        path: ':id',
        component: OrdersViewComponent,
        //TODO add ClaimPolicy
      }
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class OrdersFinancesRoutingModule { }
