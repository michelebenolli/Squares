import { EditorSize } from "./editor-size";

export interface EditorConfig<T = any> {
  title: string;
  subtitle?: string;
  size?: EditorSize;
  id?: number;
  data?: T;
  activeTab?: string;
}
