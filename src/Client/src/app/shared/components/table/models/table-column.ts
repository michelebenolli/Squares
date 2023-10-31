import { DataType } from './data-type';

export interface TableColumn<T> {
  name: string;
  value: (x: T) => any;
  type?: DataType;
  sort?: string;
  position?: 'right' | 'left';
}
