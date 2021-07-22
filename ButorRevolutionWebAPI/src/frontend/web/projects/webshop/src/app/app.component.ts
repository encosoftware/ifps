import { Component } from '@angular/core';
import { LanguageSetService } from './shared/services/language-set.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'webshop';
  constructor(
    private lngService: LanguageSetService,
  ) {
    if (this.lngService.getLocalLanguageStorage()) {
      const lng = this.lngService.getLocalLanguageStorage();
      this.lngService.setDefaultLng(lng);
    } else {
      let lng = this.lngService.defaultBrowserLng();
      this.lngService.setLocalStg(lng);
      this.lngService.setDefaultLng(lng);
    }
  }
}
