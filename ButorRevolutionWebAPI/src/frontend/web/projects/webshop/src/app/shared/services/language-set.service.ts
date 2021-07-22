import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class LanguageSetService {
  localhostKey = 'Language';
  languageValue = ['en-GB', 'hu-HU'];

  constructor(private translate: TranslateService) {
    translate.addLangs([...this.languageValue]);
  }

  defaultBrowserLng(): string {
    let dBrowValue: string;
    this.languageValue.forEach((e) => {
      if (e === this.translate.getBrowserLang()) {
        dBrowValue = e;
      } else {
        dBrowValue = this.languageValue[0];
      }
    });
    return dBrowValue;
  }

  getLocalLanguageStorage(): string {
    return localStorage.getItem(this.localhostKey);
  }

  setDefaultLng(language: string) {
    this.translate.setDefaultLang(language);
    this.translate.use(language);
  }

  setLocalStg(language: string) {
    localStorage.setItem(this.localhostKey, language);
  }

  setLanguage(language: string) {
    this.translate.use(language);
    localStorage.setItem(this.localhostKey, language);
  }

}
