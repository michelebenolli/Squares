import { Type } from '@angular/core';

export interface Action<T> {
  name: string;
  icon?: string;
  action?: (x: T) => any;
  inMenu?: boolean;
  dialog?: Type<any>;
  show?: (x: T) => boolean;
}
