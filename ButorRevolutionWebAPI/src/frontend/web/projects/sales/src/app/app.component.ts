import { Component } from '@angular/core';
import { LanguageSetService } from './core/services/language-set.service';
import { AuthService } from './core/services/auth.service';


@Component({
  selector: 'butor-root',
  templateUrl: './app.component.html'
})
export class AppComponent {

  constructor(
    private lngService: LanguageSetService,
    private authService: AuthService


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
