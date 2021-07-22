import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RolesPageComponent } from './pages/roles.page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
    path: '',
    component: RolesPageComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum['GetRoles']]
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RolesRoutingModule { }
