import { Pipe, PipeTransform } from '@angular/core';
import { ISupplyDropModel } from '../models/supply-orders.model';

@Pipe({
  name: 'supplierCheckBox'
})
export class SupplierCheckBoxPipe implements PipeTransform {

  transform(value: ISupplyDropModel[], actual: number): boolean {
    if (value && actual) {
      return !!value.find(x => x.id === actual);
    } else {
      return false;
    }
  }

}
