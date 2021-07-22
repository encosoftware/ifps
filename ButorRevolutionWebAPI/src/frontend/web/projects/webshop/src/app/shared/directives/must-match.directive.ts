import { Directive, Input } from '@angular/core';
import { NG_VALIDATORS, FormGroup, ValidationErrors } from '@angular/forms';

export const  MustMatch = (controlName: string, matchingControlName: string) => {
  return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];

      // return null if controls haven't initialised yet
      if (!control || !matchingControl) {
        return null;
      }

      // return null if another validator has already found an error on the matchingControl
      if (matchingControl.errors && !matchingControl.errors.mustMatch) {
          return null;
      }

      // set error on matchingControl if validation fails
      if (control.value !== matchingControl.value) {
          matchingControl.setErrors({ mustMatch: true });
      } else {
          matchingControl.setErrors(null);
      }
  }
}

@Directive({
  selector: '[webshopMustMatch]',
  providers: [{ provide: NG_VALIDATORS, useExisting: MustMatchDirective, multi: true }]
})
export class MustMatchDirective {
  @Input('webshopMustMatch') webshopMustMatch: string[] = [];

  validate(formGroup: FormGroup): ValidationErrors {
      return MustMatch(this.webshopMustMatch[0], this.webshopMustMatch[1])(formGroup);
  }
  constructor() { }

}
