import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable()
export class AccountService {

  baseUrl = environment.apiUrl + 'account/';

  constructor(private http: HttpClient) { }

  getPermissions(): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + 'permissions');
  }

  forgotPassword(email: string, tenant: string): Observable<void> {
    return this.http.post<void>(this.baseUrl + 'forgot-password',
      { email: email }, { headers: { tenant: tenant }});
  }

  resetPassword(resetPassword: { email: string, password: string, token: string }, tenant: string): Observable<void> {
    return this.http.post<void>(this.baseUrl + 'reset-password',
      resetPassword, { headers: { tenant: tenant }});
  }

  reconfirmEmail(email: string, tenant: string): Observable<void> {
    return this.http.post<void>(this.baseUrl + 'reconfirm-email',
      { email: email }, { headers: { tenant: tenant }});
  }
}
