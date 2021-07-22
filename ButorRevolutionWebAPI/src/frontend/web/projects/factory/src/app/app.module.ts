import { NgModule, InjectionToken } from '@angular/core';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { AppRoutingModule } from './app-routing.module';
import { StoreModule, ActionReducerMap } from '@ngrx/store';
import { HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { LoadingInterceptor } from './core/interceptors/loading.interceptor';
import { LoadingComponent } from './shared/components/loading/loading.component';
import { AuthInterceptor } from './core/interceptors/auth.interceptors';
import { reducers } from './core/store/reducers';
import { LanguageInterceptor } from './core/interceptors/language.interceptors';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ButorSharedLibModule } from 'butor-shared-lib';

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
    StoreModule.forRoot(REDUCER_TOKEN, {}),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    AppRoutingModule,
    CoreModule,
    ButorSharedLibModule,
    BrowserModule,
    BrowserAnimationsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LanguageInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: REDUCER_TOKEN,
      useValue: reducers
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
