import { EditorSize } from "./editor-size";

export interface EditorConfig<T = any> {
  title: string;
  subtitle?: string;
  size?: EditorSize;
  hideButton?: boolean;
  id?: number;
  data?: T;
}
