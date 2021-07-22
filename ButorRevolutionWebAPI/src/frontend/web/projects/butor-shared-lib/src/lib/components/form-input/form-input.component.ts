import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'butor-form-input',
  templateUrl: './form-input.component.html',
  styleUrls: ['./form-input.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormInputComponent),
      multi: true
    }
  ]
})
export class FormInputComponent implements ControlValueAccessor {

  @Input() type = 'text';
  @Input() append: string;
  @Input() appendIcon: string;
  @Input() cType = 'regular';
  @Input() name: string;
  @Input() disabled = false;
  @Input() placeholder = '';
  @Input() classAddInput: string;
  @Input() classAddInputArrow: string;
  @Input() classAddInside: string;
  @Input() webshop = false;

  @Input() value = '';

  onChangeControl: (val: any) => void;
  onTouchedControl: () => void;

  writeValue(value: string): void {
    this.value = value ? value : '';
  }
  registerOnChange(fn: any): void {
    this.onChangeControl = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouchedControl = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;

  }

  incrase() {
    this.value = JSON.stringify((+this.value + 1));
    this.onChangeControl(this.value);
  }
  decrease() {
    if (+this.value > 1) {
      this.value = JSON.stringify((+this.value - 1));
      this.onChangeControl(this.value);
    }
    this.onChangeControl(this.value);
  }

}
