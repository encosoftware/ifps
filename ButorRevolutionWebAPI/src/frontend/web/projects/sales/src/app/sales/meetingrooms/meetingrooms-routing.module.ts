import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MeetingroomsComponent } from './pages/meetingrooms.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
    path: '',
    component: MeetingroomsComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum.GetAppointments]
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MeetingroomsRoutingModule { }
