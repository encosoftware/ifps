import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'languagePipe'
})
export class LanguagePipe implements PipeTransform {

  transform(value: string): string {
    const lang = value.split('-');
    return lang[1];
  }

}
