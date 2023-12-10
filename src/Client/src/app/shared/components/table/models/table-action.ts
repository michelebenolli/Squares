import { Action } from "../../actions/models/action";

export interface TableAction<T> extends Action<T> {
  type?: 'row' | 'table' | 'multiple';
}