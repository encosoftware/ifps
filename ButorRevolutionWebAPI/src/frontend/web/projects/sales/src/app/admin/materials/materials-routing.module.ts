import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FoilsComponent } from './pages/foils/foils.component';
import { AccessoriesComponent } from './pages/accessories/accessories.component';
import { DecorboardsComponent } from './pages/decorboards/decorboards.component';
import { WorktopsComponent } from './pages/worktops/worktops.component';
import { AppliancesComponent } from './pages/appliances/appliances.component';
import { ClaimPolicyEnum } from '../../shared/clients';


const routes: Routes = [
  {
    path: '',
    component: WorktopsComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum['GetMaterials']]
    },
  },
  {
    path: 'worktops',
    component: WorktopsComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum['GetMaterials']]
    },
  },
  {
    path: 'decorboards',
    component: DecorboardsComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum['GetMaterials']]
    },
  },
  {
    path: 'foils',
    component: FoilsComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum['GetMaterials']]
    },
  },
  {
    path: 'appliances',
    component: AppliancesComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum['GetMaterials']]
    },
  },
  {
    path: 'accessories',
    component: AccessoriesComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum['GetMaterials']]
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MaterialsRoutingModule { }
