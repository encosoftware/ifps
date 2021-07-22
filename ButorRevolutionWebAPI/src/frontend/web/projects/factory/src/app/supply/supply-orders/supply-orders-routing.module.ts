import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SupplyOrdersComponent } from './pages/supply-orders.component';
import { CreateCargoComponent } from './pages/create-cargo/create-cargo.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
    path: '',
    component: SupplyOrdersComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum.GetRequiredMaterials]
    },
  },
  {
    path: 'create-cargo',
    component: CreateCargoComponent,
    data: {
      breadcrumb: 'Create Cargo',
      claims: ClaimPolicyEnum[ClaimPolicyEnum.UpdateRequiredMaterials]

    }
  },
  {
    path: 'edit-cargo/:id',
    component: CreateCargoComponent,
    data: {
      breadcrumb: 'Create Cargo',
      claims: ClaimPolicyEnum[ClaimPolicyEnum.UpdateRequiredMaterials]

    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SupplyOrdersRoutingModule { }
