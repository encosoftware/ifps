import { Component, OnInit, forwardRef } from '@angular/core';
import { getMonth, getYear, startOfMonth, getDate, getDaysInMonth, addDays, getDay, format } from 'date-fns';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { ICalendarViewModel, AbsenceDayViewModel } from '../../models/users.models';
import { UsersService } from '../../services/users.service';
import { AbsenceTypeEnum } from '../../../../shared/clients';
import { ActivatedRoute } from '@angular/router';
import { tap, map } from 'rxjs/operators';

@Component({
  selector: 'butor-days-off',
  templateUrl: './days-off.component.html',
  styleUrls: ['./days-off.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: forwardRef(() => DaysOffComponent),
    }
  ]
})
export class DaysOffComponent implements OnInit, ControlValueAccessor {

  controlOnChange: (obj: ICalendarViewModel) => void;
  controlOnTouched: () => void;
  selected = false;
  dayNames = ['M', 'T', 'W', 'T', 'F', 'S', 'S'];
  calendarId: number;
  dates: Date[] = [];
  weeks: Date[][] = [];
  selectedDates: Date[] = [];
  currentDate = new Date();
  currentMonth = getMonth(this.currentDate);
  currentYear = getYear(this.currentDate);
  yearMonth = format(new Date(this.currentYear, this.currentMonth), 'yyyy.MMMM');
  model: ICalendarViewModel = {
    sickLeave: [],
    daysOff: [],
    deleted: []
  };
  original = [];
  deleted = [];
  dateModels: AbsenceDayViewModel[] = [];
  loadingInit = false;
  constructor(private userService: UsersService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.creatCalendar(this.currentMonth, this.currentYear);
    this.calendarId = +this.route.snapshot.paramMap.get('id');
    this.userService.getDays(this.calendarId, this.currentYear, this.currentMonth).pipe(
      tap(resp => {
        this.model.daysOff = resp.daysOff;
        this.model.sickLeave = resp.sickLeave;
        this.original = [...resp.daysOff.map((c) => c.date), ...resp.sickLeave.map((c) => c.date)];
      })
    ).subscribe();
  }

  dayOff() {
    this.selectedDates.forEach((val, i) => {
      if (!!this.model.sickLeave.find(item => format(new Date(item.date), 'MM/dd/yyyy')
        === format(new Date(val), 'MM/dd/yyyy')) &&
        !!(!this.model.daysOff.find(item => format(new Date(item.date), 'MM/dd/yyyy')
          === format(new Date(val), 'MM/dd/yyyy')))) {
        this.model.sickLeave = this.model.sickLeave.filter(n => format(new Date(n.date), 'MM/dd/yyyy')
          !== format(new Date(val), 'MM/dd/yyyy'));
        this.model.daysOff = [...this.model.daysOff, {
          date: new Date(val.setHours(12)),
          absenceType: AbsenceTypeEnum.DayOff,
        }];
      } else if (!!this.model.daysOff.find(item => format(new Date(item.date), 'MM/dd/yyyy')
        === format(new Date(val), 'MM/dd/yyyy'))) {
        this.model.daysOff = [...this.model.daysOff];
      } else {
        this.model.daysOff = [...this.model.daysOff, {
          date: new Date(val.setHours(12)),
          absenceType: AbsenceTypeEnum.DayOff,
        }];
      }
    });
    this.selectedDates = [];
    this.dates = [];
  }
  sickLeave() {
    this.selectedDates.forEach(val => {
      if (!!this.model.daysOff.find(item => format(new Date(item.date), 'MM/dd/yyyy') === format(new Date(val), 'MM/dd/yyyy')) &&
        !!(!this.model.sickLeave.find(item => format(new Date(item.date), 'MM/dd/yyyy') === format(new Date(val), 'MM/dd/yyyy')))) {
        this.model.daysOff = this.model.daysOff.filter(n => format(new Date(n.date), 'MM/dd/yyyy')
          !== format(new Date(val), 'MM/dd/yyyy'));
        this.model.sickLeave = [...this.model.sickLeave, {
          date: new Date(val.setHours(12)),
          absenceType: AbsenceTypeEnum.SickLeave,
        }];
      } else if (!!this.model.sickLeave.find(item => format(new Date(item.date), 'MM/dd/yyyy')
        === format(new Date(val), 'MM/dd/yyyy'))) {
        this.model.sickLeave = [...this.model.sickLeave];
      } else {
        this.model.sickLeave = [...this.model.sickLeave, {
          date: new Date(val.setHours(12)),
          absenceType: AbsenceTypeEnum.SickLeave,
        }];
      }
    });
    this.selectedDates = [];
    this.dates = [];
  }

  deleteSelect() {
    const selected = [...this.selectedDates];
    this.selectedDates = [];
    this.dates.forEach((date) => {
      if (!!this.model.sickLeave.find(item => {
        return format(new Date(item.date), 'MM/dd/yyyy') === format(new Date(date), 'MM/dd/yyyy');
      })) {
        this.model.sickLeave =
          this.model.sickLeave.filter(n => format(new Date(n.date), 'MM/dd/yyyy') !== format(new Date(date), 'MM/dd/yyyy'));
      } else if (!!this.model.daysOff.find(item => {
        return format(new Date(item.date), 'MM/dd/yyyy') === format(new Date(date), 'MM/dd/yyyy');
      })) {
        this.model.daysOff =
          this.model.daysOff.filter(n => format(new Date(n.date), 'MM/dd/yyyy') !== format(new Date(date), 'MM/dd/yyyy'));
      }
    });

    selected.forEach(element => {
      if (!!this.original.find(item => {
        return format(new Date(element), 'MM/dd/yyyy') === format(new Date(item), 'MM/dd/yyyy');
      })) {
        this.model.deleted.push(element);
      } else {
        return;
      }

    });
    this.dates = [];
  }

  selectDay(date: Date) {
    this.dates = [...this.dates,
      new Date(date.setHours(12))];
    if (!this.selectedDates.includes(date)) {
      this.selectedDates = [...this.selectedDates, date];
    } else {
      this.selectedDates = this.selectedDates.filter(n => format(new Date(n), 'MM/dd/yyyy') !== format(new Date(date), 'MM/dd/yyyy'));
      this.dates = this.dates.filter(n => format(new Date(n), 'MM/dd/yyyy') !== format(new Date(date), 'MM/dd/yyyy'));
    }
  }


  creatCalendar(month, year) {
    this.yearMonth = format(new Date(year, month), 'yyyy.MMMM');
    const calendar: Date = new Date(year, month);
    const firstDayInc = startOfMonth(calendar);
    let isoDay = getDay(firstDayInc);
    let date = 0;
    const actualDate = getDate(addDays(firstDayInc, date));
    for (let i = 0; i < 6; i++) {
      this.weeks.push([]);
    }
    if (isoDay === 0) {
      isoDay = 6;
    }
    isoDay--;
    for (let i = 0; i < 6; i++) {
      for (let j = 0; j < 7; j++) {
        if (i === 0 && isoDay > j) {
          this.weeks[i].push(null);
        } else if (date > getDaysInMonth(calendar)) {
          this.weeks[i].push(null);
        } else if (date < getDaysInMonth(calendar) && actualDate < getDaysInMonth(calendar)) {
          this.weeks[i].push(addDays(firstDayInc, date));
          date++;
        } else {
          this.weeks[i].push(null);
        }
      }
      const emptyline = this.weeks[i].every(v => v === this.weeks[i][0]);
      if (emptyline) {
        this.weeks.splice(-1, 1);
      }
    }
    this.loadingInit = true;
  }
  trackByFn(index) {
    return index;
  }
  previusMonth() {
    this.weeks = [];
    this.currentYear = (this.currentMonth === 0) ? this.currentYear - 1 : this.currentYear;
    this.currentMonth = (this.currentMonth === 0) ? 11 : this.currentMonth - 1;
    this.creatCalendar(this.currentMonth, this.currentYear);
  }

  nextMonth() {
    this.weeks = [];
    this.currentYear = (this.currentMonth === 11) ? this.currentYear + 1 : this.currentYear;
    this.currentMonth = (this.currentMonth + 1) % 12;
    this.creatCalendar(this.currentMonth, this.currentYear);
  }

  writeValue(obj: ICalendarViewModel): void {
    if (obj) {
      this.model = obj;
    }
  }

  registerOnChange(fn: any): void {
    this.controlOnChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.controlOnTouched = fn;
  }
}

