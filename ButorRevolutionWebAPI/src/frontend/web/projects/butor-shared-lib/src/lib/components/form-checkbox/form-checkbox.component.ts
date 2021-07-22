import { Component, forwardRef, Input, Output, EventEmitter } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'butor-form-checkbox',
  templateUrl: './form-checkbox.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormCheckboxComponent),
      multi: true
    }]
})
export class FormCheckboxComponent implements ControlValueAccessor {
  @Input() label: string;
  @Input() typeBox = 'pipe';

  @Input() value = false;
  disabled = false;

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
