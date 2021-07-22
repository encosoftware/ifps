import { NgModule } from '@angular/core';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { DashboardComponent } from './pages/dashboard.component';
import { MessagesComponent } from './components/messages/messages.component';
import { TicketsComponent } from './components/tickets/tickets.component';
import { AppointmentsComponent } from './components/appointments/appointments.component';

@NgModule({
  declarations: [DashboardComponent, MessagesComponent, TicketsComponent, AppointmentsComponent],
  imports: [
    SharedModule,
    DashboardRoutingModule
  ]
})
export class DashboardModule { }
