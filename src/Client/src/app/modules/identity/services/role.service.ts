import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Role } from '../models/role';
import { RepositoryService } from 'src/app/shared/services/repository.service';

@Injectable({ providedIn: 'root' })
export class RoleService extends RepositoryService<Role> {

  constructor(http: HttpClient) {
    super(http, environment.apiUrl + 'roles/');
  }

  getPermissions(roleId?: number): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + (roleId ? `${roleId}/permissions` : 'permissions'));
  }

  updatePermissions(roleId: number, permissions: string[]): Observable<void> {
    const request = { roleId: roleId, permissions: permissions };
    return this.http.put<void>(this.baseUrl + `${request.roleId}/permissions`, request);
  }
}
