import { Component, OnInit } from '@angular/core';
import { CalendarEvent } from 'calendar-utils';
import { CalendarDateFormatter, CalendarView } from 'angular-calendar';
import { CustomDateFormatter } from '../../../sales/appointments/pages/costumDateFormatter';
import { subWeeks, addWeeks } from 'date-fns';
import { Subject } from 'rxjs';
import { CustomerAppointmentService } from '../services/customer-appointment.service';
import { select, Store } from '@ngrx/store';
import { take, tap } from 'rxjs/operators';
import { coreLoginT } from '../../../core/store/selectors/core.selector';
import { ClaimPolicyEnum } from '../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';

export interface CalendarEventSubtitle extends CalendarEvent {
  subTitle?: string;
}

@Component({
  selector: 'butor-appointments-page',
  templateUrl: './appointments-page.component.html',
  styleUrls: ['./appointments-page.component.scss'],
  providers: [
    {
      provide: CalendarDateFormatter,
      useClass: CustomDateFormatter
    }
  ]
})
export class AppointmentsPageComponent implements OnInit {

  view: CalendarView = CalendarView.Week;
  CalendarView = CalendarView;
  refresh: Subject<any> = new Subject();
  viewDate: Date = new Date();
  excludeDays: number[] = [0, 6];
  events: CalendarEventSubtitle[];
  claims: string[] = [];

  constructor(
    private store: Store<any>,
    public appointmentService: CustomerAppointmentService,
    public translateService: TranslateService
  ) { }

  ngOnInit(): void {
    this.store.pipe(
      select(coreLoginT),
      take(1),
      tap((resp) => {
        this.claims = resp.IFPSClaim;
      })
    ).subscribe();
    if(this.claims.includes(ClaimPolicyEnum[ClaimPolicyEnum.GetCustomerAppointments])){
      this.loadData();
    } else {
      this.loadPartnerData();
    }
  }

  loadData() {
    this.appointmentService.getAppointments(subWeeks(this.viewDate, 1), addWeeks(this.viewDate, 1)).subscribe(res => {
      this.events = [];
      for (let event of res) {
        this.events.push({
          title: event.subject,
          start: event.from,
          end: event.to,
          draggable: false,
          resizable: {
            afterEnd: false,
            beforeStart: false
          },
          id: event.id,
          meta: {
            address: event.address,
            customerName: event.customerName,
            notes: event.notes
          },
          subTitle: event.categoryName
        });
      }
      this.refresh.next();
    });
  }

  loadPartnerData() {
    this.appointmentService.getPartnerAppointments(subWeeks(this.viewDate, 1), addWeeks(this.viewDate, 1)).subscribe(res => {
      this.events = [];
      for (let event of res) {
        this.events.push({
          title: event.subject,
          start: event.from,
          end: event.to,
          draggable: false,
          resizable: {
            afterEnd: false,
            beforeStart: false
          },
          id: event.id,
          meta: {
            address: event.address,
            customerName: event.customerName,
            notes: event.notes
          },
          subTitle: event.categoryName
        });
      }
      this.refresh.next();
    });
  }

  hideOrShowWeekdays() {
    if (this.excludeDays.length === 0) {
      this.excludeDays = [0, 6];
    } else {
      this.excludeDays = [];
    }
  }

}
