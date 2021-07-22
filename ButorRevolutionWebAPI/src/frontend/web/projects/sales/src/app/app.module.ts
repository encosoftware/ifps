import { BrowserModule } from '@angular/platform-browser';
import { NgModule, InjectionToken, LOCALE_ID } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { StoreModule, ActionReducerMap } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from '../environments/environment';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { LanguageInterceptorService } from './core/services/language-interceptor.service';
import { AuthInterceptor } from './core/interceptors/auth.interceptors';
import { Errorhandling } from './core/interceptors/errorhandling.interceptors';
import { reducers } from './core/store/reducers';
import { LoadingInterceptor } from './core/interceptors/loading.interceptors';
import { LoadingComponent } from './core/components/loading/loading.component';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { registerLocaleData } from '@angular/common';
import localeHu from '@angular/common/locales/hu';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
registerLocaleData(localeHu);

export const REDUCER_TOKEN = new InjectionToken<ActionReducerMap<any>>('root reducer');
@NgModule({
  declarations: [
    AppComponent,
    LoadingComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CoreModule,
    BrowserAnimationsModule,
    StoreModule.forRoot(REDUCER_TOKEN, {}),
    StoreDevtoolsModule.instrument({
      maxAge: 25, // Retains last 25 states
      logOnly: environment.production, // Restrict extension to log-only mode
    }),
    CalendarModule.forRoot({ provide: DateAdapter, useFactory: adapterFactory }),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LanguageInterceptorService,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: Errorhandling,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true
    },
    {
      provide: REDUCER_TOKEN,
      useValue: reducers
    },
    {
      provide: LOCALE_ID, useFactory: (translate: TranslateService) => {
        return translate.currentLang;
      },
      deps: [TranslateService]
    },
    {
      provide: MAT_DATE_LOCALE, useFactory: (translate: TranslateService) => {
        return translate.currentLang;
      },
      deps: [TranslateService]
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
