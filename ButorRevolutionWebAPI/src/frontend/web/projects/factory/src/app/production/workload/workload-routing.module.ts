import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WorkloadComponent } from './pages/workload.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
    path: '',
    component: WorkloadComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum.GetWorkloads]
    },
}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WorkloadRoutingModule { }
