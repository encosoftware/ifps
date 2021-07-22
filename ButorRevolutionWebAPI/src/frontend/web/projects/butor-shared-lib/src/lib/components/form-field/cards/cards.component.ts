import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'butor-cards',
  templateUrl: './cards.component.html',
  styleUrls: ['./cards.component.scss'],
  providers: [
    { 
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: forwardRef(() => CardsComponent),
    }
  ]
})
export class CardsComponent implements OnInit, ControlValueAccessor {
  @Input() title: string;

  constructor() { }

  ngOnInit() {
  }

  onChangeControl: () => void;
  onTouchedControl: () => void;

  writeValue(value: string): void {
    this.title = value ? value : '';
  }
  registerOnChange(fn: any): void {
    this.onChangeControl = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouchedControl = fn;
  }

}
