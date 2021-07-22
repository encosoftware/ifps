import { NgModule } from '@angular/core';

import { AppointmentsRoutingModule } from './appointments-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { AppointmentsComponent } from './pages/appointments.component';
import { EventsEditComponent } from './components/events-edit/events-edit.component';
import { EventInfoComponent } from './components/event-info/event-info.component';


@NgModule({
  declarations: [AppointmentsComponent, EventsEditComponent, EventInfoComponent],
  imports: [
    SharedModule,
    AppointmentsRoutingModule,
  ],
  entryComponents: [EventsEditComponent],
  exports: [
    EventInfoComponent
  ]
})
export class AppointmentsModule { }
