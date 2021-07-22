import { Component, OnInit, Input, HostBinding, Output, EventEmitter, HostListener } from '@angular/core';
import { getDate, isToday } from 'date-fns';

@Component({
  selector: 'butor-calendar-day',
  templateUrl: './calendar-day.component.html',
  styleUrls: ['./calendar-day.component.scss']
})
export class CalendarDayComponent implements OnInit {
  @Input() date?: Date;
  @Input() isSelected = false;
  @Input() isDayOff = false;
  @Input() isSickLeave = false;
  @Output() selectDate = new EventEmitter<Date>();
  dateFormated: number;

  @HostBinding('class.actual-day') get isActualday() {
    return isToday(this.date);
  }
  @HostBinding('class.empty') get isEmpty() {
    return this.date === null;
  }

  ngOnInit() {
    if ((this.date)) {
      this.dateFormated = getDate(this.date);
    } else {
      this.dateFormated = null;
    }
  }

  selected() {
    if (this.date) {
      this.selectDate.emit(this.date);
    }
  }

}
