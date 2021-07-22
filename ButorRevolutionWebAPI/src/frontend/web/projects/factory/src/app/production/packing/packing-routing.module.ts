import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClaimPolicyEnum } from '../../shared/clients';
import { PackingPageComponent } from './pages/packing.component';

const routes: Routes = [
  {
      path: '',
      component: PackingPageComponent,
      data: {
          claims: ClaimPolicyEnum[ClaimPolicyEnum.GetAssemblies]
        },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PackingRoutingModule { }
