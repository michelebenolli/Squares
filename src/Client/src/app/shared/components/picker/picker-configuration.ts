import { EditorConfig } from "../editor/models/editor-config";
import { Filter } from "../filters/models/filter";

export interface PickerConfiguration<T = any> {
  service: any;
  editor: EditorConfig;
  name: (x: T) => string;
  description?: (x: T) => string | undefined;
  filters?: Filter[];
  multiple?: boolean;
}