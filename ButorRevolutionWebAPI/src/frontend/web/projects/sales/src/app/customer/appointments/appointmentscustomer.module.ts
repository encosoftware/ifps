import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { AppointmentsRoutingModule } from './appointments-routing.module';
import { AppointmentsPageComponent } from './pages/appointments-page.component';
import { AppointmentsModule } from '../../sales/appointments/appointments.module';


@NgModule({
  declarations: [AppointmentsPageComponent],
  imports: [
    SharedModule,
    AppointmentsRoutingModule,
    AppointmentsModule
  ]
})
export class AppointmentsCustomerModule { }
