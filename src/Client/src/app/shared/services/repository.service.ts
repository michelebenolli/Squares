import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { PagedList } from '../models/paged-list';
import { Patch } from '../models/patch';
import { PagedRequest } from '../models/paged-request';
import { BaseRequest } from '../models/base-request';

@Injectable({ providedIn: 'root' })
export abstract class RepositoryService<T = any, TId extends number | string = number> {

  constructor(
    protected http: HttpClient,
    protected baseUrl: string) { }

  search(request: PagedRequest): Observable<PagedList<T>> {
    return this.http.post<PagedList<T>>(this.baseUrl + 'search', request);
  }

  getAll(request?: BaseRequest): Observable<T[]> {
    return this.http.post<T[]>(this.baseUrl + 'getAll', request ?? {});
  }

  getById(id: TId): Observable<T> {
    return this.http.get<T>(this.baseUrl + id);
  }

  create(entity: T): Observable<TId> {
    return this.http.post<TId>(this.baseUrl, entity);
  }

  createMany(entities: T[]): Observable<void> {
    return this.http.post<void>(this.baseUrl + 'batch', { models: entities });
  }

  update(id: TId, entity: T): Observable<void> {
    return this.http.put<void>(this.baseUrl + id, entity);
  }

  patch(id: TId, patch: Patch | Patch[]): Observable<void> {
    patch = Array.isArray(patch) ? patch : [patch];
    return this.http.patch<void>(this.baseUrl + id, { id: id, patch: patch });
  }

  patchMany(ids: TId[], patch: Patch | Patch[]): Observable<void> {
    patch = Array.isArray(patch) ? patch : [patch];
    return this.http.patch<void>(this.baseUrl + 'batch', { ids: ids, patch: patch });
  }

  delete(id?: TId): Observable<void> {
    return id ? this.http.delete<void>(this.baseUrl + id) : of(void 0);
  }

  deleteMany(ids?: TId[]): Observable<void> {
    return ids ? this.http.delete<void>(this.baseUrl, { body: { ids: ids } }) : of(void 0);
  }

  sort(ids: number[]): Observable<void> {
    return this.http.post<void>(this.baseUrl + 'sort', { ids: ids });
  }
}
