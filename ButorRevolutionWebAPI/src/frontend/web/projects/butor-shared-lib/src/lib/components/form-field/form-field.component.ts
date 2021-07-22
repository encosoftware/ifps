import { Component, Input, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'butor-form-field',
  templateUrl: './form-field.component.html',
  styleUrls: ['./form-field.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FormFieldComponent {

  @Input() label: string;
  @Input() cType = 'regular';
  @Input() line = 'false';

}
