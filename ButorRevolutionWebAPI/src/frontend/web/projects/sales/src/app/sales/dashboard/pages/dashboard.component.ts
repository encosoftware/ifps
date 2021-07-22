import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../services/dashboard.service';
import { ITicketsModel } from '../models/tickets.model';
import { getYear, getMonth, getDate, addDays, subDays } from 'date-fns';
import { IAppointmentsModel } from '../models/appointments.model';
import { IMessageViewModel } from '../models/messages.model';
import { ClaimPolicyEnum } from '../../../shared/clients';
import { Store, select } from '@ngrx/store';
import { take, tap } from 'rxjs/operators';
import { coreLoginT } from '../../../core/store/selectors/core.selector';

@Component({
  selector: 'butor-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  appointmentsDataSource: IAppointmentsModel[] = [];
  allTicketsDataSource: ITicketsModel[] = [];
  ownTicketsDataSource: ITicketsModel[] = [];
  currentDate = new Date();
  userId = 1;
  claims: string[] = [];
  messages: IMessageViewModel[];


  constructor(
    private service: DashboardService,
    private store: Store<any>
  ) { }

  ngOnInit() {
    this.store.pipe(
      select(coreLoginT),
      take(1),
      tap((resp) => {
        this.claims = resp.IFPSClaim;
        this.userId = parseInt(resp.UserId);
      })
    ).subscribe();
    if (this.claims.includes(ClaimPolicyEnum[ClaimPolicyEnum.GetAppointments])) {
       this.service.getAppointmentsByDate(
        this.currentDate,
        this.currentDate,
        this.userId
      ).subscribe(res => this.appointmentsDataSource = res);
    } else if (this.claims.includes(ClaimPolicyEnum[ClaimPolicyEnum.GetPartnerAppointments])) {
      this.service.getPartnerAppointmentsByDate(
        this.currentDate,
        this.currentDate,
      ).subscribe(res => this.appointmentsDataSource = res);
    }
    this.service.getTickets().subscribe(res => this.allTicketsDataSource = res);
    this.service.getTicketsByUserId().subscribe(res => this.ownTicketsDataSource = res);
  }

  nextDay(): void {
    this.currentDate = addDays(this.currentDate, 1);
    this.service.getAppointmentsByDate(
      this.currentDate,
      this.currentDate,
      this.userId
    ).subscribe(res => this.appointmentsDataSource = res);
  }

  previousDay(): void {
    this.currentDate = subDays(this.currentDate, 1);
    this.service.getAppointmentsByDate(
      this.currentDate,
      this.currentDate,
      this.userId
    ).subscribe(res => this.appointmentsDataSource = res);
  }
}
