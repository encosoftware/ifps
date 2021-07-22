import { AbstractControl, ValidatorFn, Validator, NG_VALIDATORS } from '@angular/forms'
import { Directive, Input } from '@angular/core';

function validateOrderAmountFactory(packageSize: number, minAmount: number): ValidatorFn {
  return (c: AbstractControl) => {
    let isValid = (c.value * packageSize) >= minAmount;

    if (isValid) {
      return null;
    } else {
      return {
        orderAmount: {
          valid: false
        }
      };
    }
  }
}

@Directive({
  selector: "[amountValidator]",
  providers: [
    { provide: NG_VALIDATORS, useExisting: OrderAmountValidatorDirective, multi: true }
  ]
})
export class OrderAmountValidatorDirective implements  Validator {
  @Input("packageSize") packageSize: number;
  @Input("minAmount") minAmount: number;
  validator: ValidatorFn;

  constructor() { }

  validate(control: AbstractControl): import("@angular/forms").ValidationErrors {
    return validateOrderAmountFactory(this.packageSize, this.minAmount)(control);
  }
}