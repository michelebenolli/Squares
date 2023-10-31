import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { NotificationService } from '../services/notification.service';

@Injectable()
export class PermissionGuard {
  constructor(
    private authService: AuthService,
    private notification: NotificationService) { }

  async canActivate(route: ActivatedRouteSnapshot): Promise<boolean> {

    const permissions = route.data['permission'] ?? [];
    const isAuthorized = await this.authService.isAuthorized(permissions);

    if (!isAuthorized) {
      this.notification.warning("Non hai l'autorizzazione per accedere a questa risorsa");
    }
    return isAuthorized;
  }
}
