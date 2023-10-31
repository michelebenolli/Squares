import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LocalStorageService } from './local-storage.service';
import { Token } from '../models/token';
import { Login } from '../models/login';
import { CurrentUser } from '../models/current-user';

const CLAIMS = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private baseUrl = environment.apiUrl;
  private token = new BehaviorSubject<string | null>(this.getStorageToken);
  private permissions = new BehaviorSubject<string[] | null>(this.getStoragePermissions);

  constructor(
    private http: HttpClient,
    private localStorage: LocalStorageService,
    private router: Router) { }

  public login(login: Login, tenant: string): Observable<Token> {
    return this.http.post<Token>(this.baseUrl + 'tokens', login,
      { headers: { 'tenant': tenant } })
      .pipe(tap(x => this.setStorageToken(x)));
  }

  public logout() {
    this.setStorageToken(null);
    this.router.navigateByUrl('/login');
  }

  public get isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    return token != null && !new JwtHelperService().isTokenExpired(token);
  }

  public async isAuthorized(permissions: string[]) {
    if (permissions.length === 0) return true;
    if (!this.isAuthenticated) return false;

    const userPermissions = this.getPermissions;
    return userPermissions != null && permissions.every(x =>
      userPermissions.some(y => x?.toUpperCase() === y?.toUpperCase()));
  }

  public get getToken(): string | null {
    return this.token.getValue();
  }

  public get getPermissions(): string[] | null {
    return this.permissions.getValue();
  }

  public get getUser(): CurrentUser {
    const token = this.getDecodedToken();
    return {
      id: token[CLAIMS + 'nameidentifier'],
      firstName: token[CLAIMS + 'name'],
      lastName: token[CLAIMS + 'surname'],
      email: token[CLAIMS + 'emailaddress'],
      phone: token[CLAIMS + 'mobilephone'],
      tenant: token['tenant'],
      image: token['image_url']
    }
  }

  private setStorageToken(data: Token | null) {
    const permissions = data?.permissions.map(x => x.replace('Permissions.', ''));
    this.localStorage.setItem('token', data?.token);
    this.localStorage.setItem('refreshToken', data?.refreshToken);
    this.localStorage.setItems('permissions', permissions);

    this.token.next(data?.token ?? null);
    this.permissions.next(permissions ?? null);
  }

  private get getStorageToken(): string | null {
    return this.localStorage.getItem('token');
  }

  public get getStoragePermissions(): string[] | null {
    return this.localStorage.getItems('permissions');
  }

  private getDecodedToken(): any {
    const token = this.getStorageToken;
    return token ? new JwtHelperService().decodeToken(token) : null;
  }
}
