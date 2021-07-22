import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import { hmrBootstrap } from './hmr';
import { API_BASE_URL } from './app/shared/clients';


fetch('assets/settings.json').then(async res => {
  const config = await res.json();

  const providers = [
    { provide: API_BASE_URL, useValue: config.apiBaseUrl }
  ];

  if (environment.production) {
    enableProdMode();
  }

  const bootstrap = () => platformBrowserDynamic(providers).bootstrapModule(AppModule);

  if (environment.hmr) {
    // tslint:disable-next-line:no-string-literal
    if (module['hot']) {
      hmrBootstrap(module, bootstrap);
    } else {
      console.error('HMR is not enabled for webpack-dev-server!');
      console.log('Are you using the --hmr flag for ng serve?');
    }
  } else {
    bootstrap().catch(err => console.log(err));
  }
});