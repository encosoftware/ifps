import { Component, OnInit, forwardRef, Input } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

@Component({
  selector: "butor-form-textarea",
  templateUrl: "./form-textarea.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormTextareaComponent),
      multi: true
    }
  ]
})
export class FormTextareaComponent implements OnInit, ControlValueAccessor {
  onChangeControl: (val: any) => void;
  onTouchedControl: () => void;
  @Input() name: string;
  disabled = false;
  value = "";
  constructor() {}

  ngOnInit() {}
  writeValue(value: string): void {
    this.value = value ? value : "";
  }
  registerOnChange(fn: any): void {
    this.onChangeControl = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouchedControl = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.disabled = !isDisabled;
  }
}
