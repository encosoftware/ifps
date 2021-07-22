import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { TranslateService } from '@ngx-translate/core';
import { LanguageSetService } from '../../shared/services/language-set.service';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';


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
