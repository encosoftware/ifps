import { NgModule } from '@angular/core';

import { MeetingroomsRoutingModule } from './meetingrooms-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { MeetingroomsComponent } from './pages/meetingrooms.component';
import { AppointmentsModule } from '../appointments/appointments.module';

@NgModule({
  declarations: [
    MeetingroomsComponent
  ],
  imports: [
    SharedModule,
    MeetingroomsRoutingModule,
    AppointmentsModule
  ]
})
export class MeetingroomsModule { }
