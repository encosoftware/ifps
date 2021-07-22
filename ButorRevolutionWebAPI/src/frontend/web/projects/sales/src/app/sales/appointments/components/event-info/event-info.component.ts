import { Component, OnInit, Input, TemplateRef, HostListener, ChangeDetectionStrategy } from '@angular/core';
import { CalendarEventSubtitle } from '../../pages/appointments.component';
import { of } from 'rxjs';
import { delay } from 'rxjs/operators';

@Component({
  selector: 'butor-event-info',
  templateUrl: './event-info.component.html',
  styleUrls: ['./event-info.component.scss']
})
export class EventInfoComponent implements OnInit {
  isOpen = false;
  isFocused = false;
  @Input() detailsTemplate: TemplateRef<any>;
  @Input() event: CalendarEventSubtitle;


  @HostListener('mouseenter') openTooltip() {
    this.isOpen = true;
    this.isFocused = true;
  }
  @HostListener('mouseleave') closeTooltip() {
    this.isFocused = false;
    const delayObservable = of('').pipe(delay(200));
    delayObservable.subscribe(s => {
      if (!this.isFocused) {
        this.isOpen = false;
      }
    });
  }

  constructor() { }

  ngOnInit() {
  }

}
