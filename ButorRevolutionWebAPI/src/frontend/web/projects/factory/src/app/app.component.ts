import { Component } from '@angular/core';
import { LanguageSetService } from './core/services/language-set.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'factory';
  
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
