import {
  APP_INITIALIZER,
  ApplicationConfig,
  importProvidersFrom,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter, withRouterConfig } from '@angular/router';
import { routes } from './app.routes';
import {
  HttpClientModule,
  provideHttpClient,
  withInterceptors,
} from '@angular/common/http';
import { provideTranslateHttpLoader } from '@ngx-translate/http-loader';
import { provideTranslateService } from '@ngx-translate/core';
import { authInterceptorsInterceptor } from './Core/Interceptors/auth-interceptors.interceptor';
import { AuthService } from './Core/Services/Auth.service';
import { provideAnimations } from '@angular/platform-browser/animations';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(),
    {
      provide: APP_INITIALIZER,
      deps: [AuthService],
      useFactory: appInitializer,
      multi: true,
    },
    provideAnimations(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(
      routes,
      withRouterConfig({
        onSameUrlNavigation: 'reload',
      })
    ),
    importProvidersFrom(HttpClientModule),
    provideTranslateService({
      defaultLanguage: 'pl',
    }),
    provideHttpClient(withInterceptors([authInterceptorsInterceptor])),
    provideTranslateService({
      loader: provideTranslateHttpLoader({
        prefix: 'i18n/',
        suffix: '.json',
      }),
    }),
  ],
};
function appInitializer(authService: AuthService) {
  return () => {
    authService
      .Refresh()
      .then(() => {})
      .finally(() => {
        document.getElementById('global-loader')?.remove();
      });
  };
}
