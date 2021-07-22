import { Pipe, PipeTransform } from '@angular/core';
import { IModuleViewModel } from '../../models/users.models';

@Pipe({
  name: 'checkboxPipe'
})
export class CheckboxPipePipe implements PipeTransform {

  public transform(val: IModuleViewModel): string {
    const checked = val.claims.filter(x => x.isChecked).length;
    const count = val.claims.length;

    if (checked === 0) {
      return 'a';
    } else if (checked > 0 && checked < count) {
      return 'b';
    } else if (checked === count) {
      return 'c';
    }

    return '';
  }

}
