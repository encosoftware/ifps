import { Component, Input, forwardRef, Output, EventEmitter } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-radiobutton',
  templateUrl: './radiobutton.component.html',
  styleUrls: ['./radiobutton.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RadiobuttonComponent),
      multi: true
    }
  ]
})
export class RadiobuttonComponent implements ControlValueAccessor {
  @Input() label: string;
  @Input() name = 'radio';
  @Input() disabled = false;
  @Input() classAddInput: string;
  @Input() classAddbuttonSpan: string;
  @Input() classAddLabel: string;
  @Input() classAddGroup: string;


  value = null;

  constructor() { }

  @Output() change = new EventEmitter<boolean>();

  getValue() {
    return this.value;
  }
  setValue(val: boolean) {
    this.value = val;
  }

  onChangeControl = (_: any) => { };
  onTouchedControl = () => { };

  onCheckboxChange(event: boolean) {
    this.change.emit(event);
    this.onChangeControl(event);
    this.writeValue(event);
  }

  writeValue(check: boolean): void {
    this.value = (check === true) ? check : false;
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
}
