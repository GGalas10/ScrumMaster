import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { HttpClient, HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import {provideTranslateService, TranslateLoader} from "@ngx-translate/core";
import {TranslateHttpLoader} from '@ngx-translate/http-loader';
import { authInterceptorsInterceptor } from './Core/Interceptors/auth-interceptors.interceptor';

const httpLoaderFactory: (http: HttpClient) => TranslateHttpLoader = (http: HttpClient) =>
  new TranslateHttpLoader(http, './i18n/', '.json');



export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes),
    importProvidersFrom(HttpClientModule),
    provideTranslateService({
      defaultLanguage: 'pl'
    }),
    provideHttpClient(
      withInterceptors([authInterceptorsInterceptor])
    ),
    provideTranslateService({
      loader:
      {
        provide: TranslateLoader,
        useFactory : httpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ]
};
