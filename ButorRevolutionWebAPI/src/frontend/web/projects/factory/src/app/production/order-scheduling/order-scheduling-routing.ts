import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrderSchedulingPageComponent } from './pages/order-scheduling.page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: OrderSchedulingPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetOrderSchedulings]
          },
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class OrderSchedulingRoutingModule { }
