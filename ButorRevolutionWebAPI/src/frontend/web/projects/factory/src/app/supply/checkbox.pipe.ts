import { Pipe, PipeTransform } from '@angular/core';
import { ISupplyOrderListViewModel } from './supply-orders/models/supply-orders.model';

@Pipe({
  name: 'supplyCheckboxPipe',
  pure: false
})
export class SupplyCheckboxPipe implements PipeTransform {

  public transform(val: ISupplyOrderListViewModel[]): string {
    const checked = val.filter(x => x.isChecked).length;
    const count = val.length;
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
