import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'isActive'
})
export class IsActivePipe implements PipeTransform {

  transform(selected: any, args?: any): boolean {
    return selected === args;
  }
}

@Pipe({
  name: 'lessThan'
})
export class LessThanPipe implements PipeTransform {

  transform(subj: number, num: number): boolean {
    return subj <= num;
  }
}
