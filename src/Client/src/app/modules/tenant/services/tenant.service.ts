import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Tenant } from '../models/tenant';
import { RepositoryService } from 'src/app/shared/services/repository.service';

@Injectable({ providedIn: 'root' })
export class TenantService extends RepositoryService<Tenant, string>  {

  constructor(http: HttpClient) {
    super(http, environment.apiUrl + 'tenants/');
  }

  getCurrent(): Observable<string> {
    const hostname = window.location.hostname;
    return this.http.get(this.baseUrl + `current/${hostname}`, { responseType: 'text' })
      .pipe(map((response: string) => response));
  }
}
