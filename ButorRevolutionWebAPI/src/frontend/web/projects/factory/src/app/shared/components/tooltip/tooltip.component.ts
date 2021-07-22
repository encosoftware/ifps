import { OnInit, TemplateRef, Input, Component, HostListener, Output, EventEmitter } from '@angular/core';


@Component({
  selector: 'butor-tooltip',
  templateUrl: './tooltip.component.html',
  styleUrls: ['./tooltip.component.scss']
})
export class TooltipComponent implements OnInit {
  isOpen = false;
  @Input() template: TemplateRef<any>;
  @Output() open = new EventEmitter<boolean>();

  @HostListener('mouseenter') openTooltip() {
    this.isOpen = true;
    this.open.emit(true);
  }
  @HostListener('mouseleave') closeTooltip() {
    this.isOpen = false;
  }
  constructor() { }

  ngOnInit() {
  }
}