import { Component, OnInit } from '@angular/core';
import { CalendarView, CalendarEvent, CalendarEventTimesChangedEvent, CalendarDateFormatter } from 'angular-calendar';
import { Subject, forkJoin } from 'rxjs';
import { addWeeks, subWeeks, addHours } from 'date-fns';
import { MatDialog } from '@angular/material/dialog';
import { EventsEditComponent } from '../components/events-edit/events-edit.component';
import { CustomDateFormatter } from './costumDateFormatter';
import { AppointmentsService } from '../services/appointment.service';
import { SnackbarService } from 'butor-shared-lib';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../../core/store/selectors/core.selector';
import { take } from 'rxjs/operators';
import { ClaimPolicyEnum } from '../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';


export interface CalendarEventSubtitle extends CalendarEvent {
  subTitle?: string;
}

@Component({
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.scss'],
  providers: [
    {
      provide: CalendarDateFormatter,
      useClass: CustomDateFormatter
    }
  ]
})
export class AppointmentsComponent implements OnInit {

  view: CalendarView = CalendarView.Week;
  CalendarView = CalendarView;
  refresh: Subject<any> = new Subject();
  viewDate: Date = new Date();
  excludeDays: number[] = [0, 6];
  events: CalendarEventSubtitle[];
  claimPolicyEnum = ClaimPolicyEnum;
  userCompany: string;

  constructor(
    public dialog: MatDialog,
    public appointmentService: AppointmentsService,
    public snackBar: SnackbarService,
    private store: Store<any>,
    public translateService: TranslateService
  ) { }

  ngOnInit(): void {
    this.loadData();
    this.store.pipe(
      select(coreLoginT),
      take(1)
    ).subscribe(res => this.userCompany = res.CompanyId);
  }

  loadData() {
    this.appointmentService.getAppointments(subWeeks(this.viewDate, 1), addWeeks(this.viewDate, 1)).subscribe(res => {
      this.events = [];
      for (let event of res) {
        this.events.push({
          title: event.subject,
          start: event.from,
          end: event.to,
          draggable: true,
          resizable: {
            afterEnd: true,
            beforeStart: true
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

  eventTimesChanged(event: CalendarEventTimesChangedEvent): void {
    if (
      event.newStart.getFullYear() !== event.newEnd.getFullYear() ||
      event.newStart.getMonth() !== event.newEnd.getMonth() ||
      event.newStart.getDate() !== event.newEnd.getDate()) {
      this.snackBar.customMessage(this.translateService.instant('snackbar.diffDays'));
      return;
    }
    if (this.events.findIndex(x => (x.id === event.event.id
      && x.start.getTime() === event.newStart.getTime()
      && x.end.getTime() === event.newEnd.getTime())) > -1) {
      return;
    }
    this.events = this.events.map(iEvent => {
      if (iEvent === event.event) {
        return {
          ...event.event,
          start: event.newStart,
          end: event.newEnd
        };
      }
      return iEvent;
    });
    this.appointmentService.putDragedAppointment(+event.event.id, event.newStart, event.newEnd).subscribe(res => {
      this.loadData();
      this.snackBar.customMessage(this.translateService.instant('snackbar.success'));
    });
  }

  editEvent(id: number): void {
    forkJoin([
      this.appointmentService.getAppointment(id),
      this.appointmentService.getVenues(),
      this.appointmentService.getSalesPersons(),
      this.appointmentService.getAppointmentTypes(),
      this.appointmentService.getCountries()
    ]).subscribe(([app, ven, sale, type, coun]) => {
      const dialogRef = this.dialog.open(EventsEditComponent, {
        width: '98rem',
        data: {
          date: app.date,
          data: app,
          venues: ven,
          sales: sale,
          types: type,
          countries: coun
        }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result !== undefined) {
          this.appointmentService.putAppointment(id, result).subscribe(() => {
            this.snackBar.customMessage(this.translateService.instant('snackbar.saved'));
            this.loadData();
          });
        }
      });
    });
  }

  addEvent(evnt): void {

    forkJoin([
      this.appointmentService.getVenues(),
      this.appointmentService.getSalesPersons(),
      this.appointmentService.getAppointmentTypes(),
      this.appointmentService.getCountries()
    ]).subscribe(([ven, sale, type, coun]) => {
      const dialogRef = this.dialog.open(EventsEditComponent, {
        width: '98rem',
        data: {
          venues: ven,
          sales: sale,
          types: type,
          countries: coun,
          date: evnt.date,
          fromDate: evnt.date,
          toDate: addHours(evnt.date, 2),
        }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result !== undefined) {
          this.appointmentService.postAppointment(result).subscribe(() => {
            this.snackBar.customMessage(this.translateService.instant('snackbar.success'));
            this.loadData();
          },
            (err) => this.snackBar.customMessage('snackbar.wrongStatus'));
        }
      });
    });
  }

  deleteEvent(id: number) {
    this.appointmentService.deleteAppointment(id).subscribe(() => {
      this.snackBar.customMessage(this.translateService.instant('snackbar.deleted'));
      this.loadData();
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
