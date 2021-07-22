import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './pages/dashboard.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum.GetAppointments]
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
