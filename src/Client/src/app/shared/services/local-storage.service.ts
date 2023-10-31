import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LocalStorageService {

  public setItems(key: string, value?: string[]) {
    this.setItem(key, value?.join(','));
  }

  public setItem(key: string, value?: string) {
    if (value) localStorage.setItem(key, value);
    else localStorage.removeItem(key);
  }

  public getItems(key: string) {
    return localStorage.getItem(key)?.split(',') ?? null;
  }

  public getItem(key: string) {
    return localStorage.getItem(key);
  }

  public removeItem(key: string) {
    localStorage.removeItem(key);
  }

  public clear() {
    localStorage.clear();
  }
}
