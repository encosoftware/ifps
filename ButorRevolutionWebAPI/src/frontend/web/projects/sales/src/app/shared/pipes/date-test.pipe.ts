import { Pipe, PipeTransform } from '@angular/core';
import { isToday, isPast, isFuture, isWeekend } from 'date-fns';

@Pipe({
  name: 'dateTest'
})
export class DateTestPipe implements PipeTransform {

  transform(dateX: Date, args?: string): boolean {
    const date = new Date(dateX);
    switch(args) {
  case "past":
    return isPast(date);
    break;
  case "today":
    return isToday(date);
    break;
  case "future":
    return isFuture(date);
    break;
  case "weekend":
    return isWeekend(date);
    break;
  default:
    return false;
}
  }

}
