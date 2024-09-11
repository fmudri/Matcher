import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';

export const appConfig: ApplicationConfig = {
  // Adding HTTP client to our providers
  providers: [
  provideRouter(routes),
  provideHttpClient(),
  provideAnimations()
  ]
};
