import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { CalendarView } from 'angular-calendar';
import { CalendarEvent } from 'calendar-utils';
import { IMeetingRoomsViewModel } from '../models/meeting-rooms.model';
import { MeetingroomsService } from '../services/meetingrooms.service';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../../core/store/selectors/core.selector';
import { take, tap, switchMap, map } from 'rxjs/operators';
import { subWeeks, addWeeks } from 'date-fns';
import { TranslateService } from '@ngx-translate/core';

export interface CalendarEventSubtitle extends CalendarEvent {
  subTitle?: string;
}

@Component({
  selector: 'butor-meetingrooms',
  templateUrl: './meetingrooms.component.html',
  styleUrls: ['./meetingrooms.component.scss']
})
export class MeetingroomsComponent implements OnInit {

  dataSource: IMeetingRoomsViewModel[];
  selectedMeetingRoom: IMeetingRoomsViewModel;
  companyId: number;

  viewDate: Date = new Date();
  view: CalendarView = CalendarView.Week;

  activeDayIsOpen = true;
  isOpen = false;

  excludeDays: number[] = [0, 6];
  refresh: Subject<any> = new Subject();

  events: CalendarEventSubtitle[] = [];

  selectedMeetingRoomId: number;

  constructor(
    public dialog: MatDialog,
    private service: MeetingroomsService,
    private store: Store<any>,
    public translateService: TranslateService
  ) { }

  ngOnInit() {
    this.store.pipe(
      select(coreLoginT),
      take(1),
      tap((resp) => {
        this.companyId = +resp.CompanyId;
      }),
      switchMap(storeResp =>
        this.service.getMeetingRoomsList(+storeResp.CompanyId).pipe(
          map(res => this.dataSource = res)
        )
      ),
    ).subscribe();
  }

  setView(view: CalendarView) {
    this.view = view;
  }

  showWeekendDays(): void {
    if (this.excludeDays.length === 0) {
      this.excludeDays = [0, 6];
    } else {
      this.excludeDays = [];
    }
  }

  showAppointments(id: number): void {
    this.selectedMeetingRoomId = id;
    this.loadData();
  }

  loadData() {
    this.service.getAppointments(this.selectedMeetingRoomId, subWeeks(this.viewDate, 1), addWeeks(this.viewDate, 1)).subscribe(res => {
      this.events = res;
      this.selectedMeetingRoom = this.dataSource[this.dataSource.findIndex(x => x.id === this.selectedMeetingRoomId)];
    });
  }
}
