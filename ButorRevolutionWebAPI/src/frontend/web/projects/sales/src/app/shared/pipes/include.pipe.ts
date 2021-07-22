import { Pipe, PipeTransform } from '@angular/core';
import { format } from 'date-fns';
import { AbsenceDayViewModel } from '../../admin/users/models/users.models';

@Pipe({
  name: 'include'
})
export class IncludePipe implements PipeTransform {

  transform(value: Array<any>, arg: number | Date | string | boolean, select: string): boolean {
    const trueOrFalse = value.includes(arg);
    if (arg instanceof Date && select === 'select') {
      return !!value.find(item => {
        return format(new Date(item), 'MM/dd/yyyy')
          === format(new Date(arg), 'MM/dd/yyyy')
      });

    } else if (arg instanceof Date) {
      return !!value.find((item: AbsenceDayViewModel) => {
        return format(new Date(item.date), 'MM/dd/yyyy') === format(new Date(arg), 'MM/dd/yyyy')
      });
    }

    return trueOrFalse;
  }

}

@Pipe({
  name: 'notinclude'
})
export class NotIncludePipe implements PipeTransform {

  transform(value: Array<any>, arg: number | Date | string | boolean, select: string): boolean {
    if (value.length === 1) {
      return !value.includes(arg);
    } else if (value.length > 1) {
      return true;
    } else {
      return false;
    }

  }

}