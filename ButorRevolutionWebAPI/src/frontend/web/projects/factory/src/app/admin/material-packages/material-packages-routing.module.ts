import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MaterialPackagePageComponent } from './pages/material-packages-page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
      path: '',
      component: MaterialPackagePageComponent,
      data: {
        claims: ClaimPolicyEnum[ClaimPolicyEnum.GetMaterialPackages]
      },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MaterialPackageRoutingModule { }
