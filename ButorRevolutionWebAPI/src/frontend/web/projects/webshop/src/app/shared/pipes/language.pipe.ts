import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'languagePipe'
})
export class LanguagePipe implements PipeTransform {

  transform(value: string[], language: string): string {
    const selected = value.find(x => x === language).split('-');
    return selected[1];
  }

}
