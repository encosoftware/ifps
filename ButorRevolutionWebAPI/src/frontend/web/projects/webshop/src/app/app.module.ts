import { BrowserModule } from '@angular/platform-browser';
import { NgModule, InjectionToken } from '@angular/core';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { CoreModule } from './core/core.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ActionReducerMap, StoreModule } from '@ngrx/store';
import { reducers } from './login/store/reducers';
import { HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { LanguageInterceptorService } from './core/interceptors/language.interceptor';
import { AuthInterceptor } from './core/interceptors/auth.interceptors';
import { LoadingInterceptor } from './core/interceptors/loading.interceptor';
import { LoadingComponent } from './core/components/loading/loading.component';

export const REDUCER_TOKEN = new InjectionToken<ActionReducerMap<any>>('root reducer');
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}


@NgModule({
  declarations: [
    AppComponent,
    LoadingComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    CoreModule,
    StoreModule.forRoot(REDUCER_TOKEN, {}),
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
      provide: REDUCER_TOKEN,
      useValue: reducers
    },
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
      useClass: LoadingInterceptor,
      multi: true
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
