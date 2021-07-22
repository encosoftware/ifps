import { Pipe, PipeTransform } from '@angular/core';
import { differenceInMinutes, isAfter, isBefore } from 'date-fns';

@Pipe({
  name: 'percantage'
})
export class PercantagePipe implements PipeTransform {

  transform(date: Date, newDate: Date): number {
    const diffMin = differenceInMinutes(date, newDate);
    const after = isAfter(date, newDate);
    const before = isBefore(date, newDate);
    const percantage = Math.floor((diffMin / 60 / 72) * 100);
    if (after) {
      if (percantage > 100) {
        return 100;
      } else if (percantage  < 0) {
        return 0;
      }
      return percantage;
    } else if (before) {
      return 0;
    }
  }


}
