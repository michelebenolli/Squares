import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';
import { Role } from '../models/role';
import { RepositoryService } from 'src/app/shared/services/repository.service';

@Injectable({ providedIn: 'root' })
export class UserService extends RepositoryService<User> {

  constructor(http: HttpClient) {
    super(http, environment.apiUrl + 'users/');
  }

  getRoles(id: number): Observable<Role[]> {
    return this.http.get<Role[]>(this.baseUrl + `${id}/roles`);
  }

  toggle(id: number, active: boolean) {
    return this.http.put<void>(this.baseUrl + `${id}/toggle`, { id: id, active: active });
  }
}
