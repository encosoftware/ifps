import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CamerasPageComponent } from './pages/cameras-page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
      path: '',
      component: CamerasPageComponent,
      data: {
        claims: ClaimPolicyEnum[ClaimPolicyEnum.GetCameras]
      },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CamerasRoutingModule { }
