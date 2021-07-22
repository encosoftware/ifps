import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { LanguageSetService } from './language-set.service';

@Injectable({
  providedIn: 'root'
})
export class LanguageInterceptorService implements HttpInterceptor {

  constructor(private translate: TranslateService, private lngService: LanguageSetService) {
  }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.translate.currentLang) {
      const languageReq = req.clone({
        setHeaders: {
          'Accept-Language': this.translate.currentLang
        }
      });
      return next.handle(languageReq);
    }
    return next.handle(req);
  }
}
