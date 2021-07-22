import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
    name: 'secToHourMinSec',
    pure: false
})
export class SecConverterPipe implements PipeTransform {
  transform(value: number): Date {
    let d = new Date(0, 0, 0, 0, 0, 0);
    d.setSeconds(value);
    return d;
  }
}
