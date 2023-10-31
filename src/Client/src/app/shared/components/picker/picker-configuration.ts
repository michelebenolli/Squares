import { EditorConfig } from "src/app/shared/components/editor/models/editor-config";
import { Filter } from "src/app/shared/components/filters/models/filter";

export interface PickerConfiguration<T> {
  service: any;
  editor: EditorConfig;
  name: (x: T) => string;
  description?: (x: T) => string | undefined;
  filters?: Filter[];
  multiple?: boolean;
}
