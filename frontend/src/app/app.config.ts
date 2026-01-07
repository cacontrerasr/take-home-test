import { ApplicationConfig, provideZoneChangeDetection, InjectionToken, inject } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, HttpInterceptorFn, withInterceptors } from '@angular/common/http';

import { routes } from './app.routes';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');
export const API_KEY = new InjectionToken<string>('API_KEY');

const apiKeyInterceptor: HttpInterceptorFn = (req, next) => {
  const apiKey = inject(API_KEY);
  return next(req.clone({ setHeaders: { 'X-Api-Key': apiKey } }));
};

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([apiKeyInterceptor])),
    { provide: API_BASE_URL, useValue: 'http://localhost:5050' },
    { provide: API_KEY, useValue: 'local-dev-key' },
  ],
};
