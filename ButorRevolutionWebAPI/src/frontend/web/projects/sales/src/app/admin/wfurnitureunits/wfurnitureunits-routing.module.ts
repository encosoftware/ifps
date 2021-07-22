import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WFUPageComponent } from './pages/wfurnitureunits-page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
      path: '',
      component: WFUPageComponent,
       data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum.GetFurnitureUnits]
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WFURoutingModule { }
