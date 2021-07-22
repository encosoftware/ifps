import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WorkstationsPageComponent } from './pages/workstations-page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
      path: '',
      component: WorkstationsPageComponent,
      data: {
        claims: ClaimPolicyEnum[ClaimPolicyEnum.GetWorkstations]
      },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WorkstationsRoutingModule { }
