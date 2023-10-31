import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import setDefaultOptions from 'date-fns/setDefaultOptions';
import { it } from 'date-fns/locale';

// Set global locale for date-fns
setDefaultOptions({ locale: it });

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic()
    .bootstrapModule(AppModule)
    .catch((err) => console.error(err));
