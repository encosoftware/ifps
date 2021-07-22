import { Component, OnInit, OnDestroy, Input, Output, EventEmitter, HostListener, TemplateRef } from '@angular/core';

@Component({
  selector: 'factory-new-window',
  templateUrl: './new-window.component.html',
  styleUrls: ['./new-window.component.scss']
})
export class NewWindowComponent implements OnInit, OnDestroy {
  @Input() template: TemplateRef<any>;
  @Output() openClick = new EventEmitter<boolean>();
  @Input() isOpen = false;
  isDragged = false;

  @HostListener('click') openTooltip() {
    if (this.isOpen) {
      if (this.isDragged) {
        this.isDragged = false;
      } else {
        this.isOpen = false;
      }

    } else {
      this.isOpen = true;
      this.openClick.emit(this.isOpen);
    }
  }

  elementDrag() {
    this.isDragged = true;
  }


  constructor(
  ) { }


  ngOnInit() {
  }

  ngOnDestroy() {
  }
}