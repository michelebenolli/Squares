import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class NotificationService {

  constructor(
    private notification: MessageService,
    private translate: TranslateService) { }

  success(message?: string, title?: string) {
    this.translation([message, title]).subscribe(x => {
      this.notification.add({ severity: 'success', summary: title ? x[title] : undefined, detail: message ? x[message] : undefined });
    });
  }

  error(message?: string, title?: string) {
    this.translation([message, title]).subscribe(x => {
      this.notification.add({ severity: 'error', summary: title ? x[title] : undefined, detail: message ? x[message] : undefined });
    });
  }

  info(message?: string, title?: string) {
    this.translation([message, title]).subscribe(x => {
      this.notification.add({ severity: 'info', summary: title ? x[title] : undefined, detail: message ? x[message] : undefined });
    });
  }

  warning(message?: string, title?: string) {
    this.translation([message, title]).subscribe(x => {
      this.notification.add({ severity: 'warning', summary: title ? x[title] : undefined, detail: message ? x[message] : undefined });
    });
  }

  private translation(keys: (string | undefined)[]): Observable<any> {
    return this.translate.get(keys.filter(x => x) as string[]);
  }
}
