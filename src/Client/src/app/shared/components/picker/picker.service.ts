import { Injectable, Type } from '@angular/core';
import { PickerConfiguration } from './picker-configuration';
import { PickerEditorComponent } from './picker-editor/picker-editor.component';
import { EditorService } from '../editor/editor.service';

@Injectable({ providedIn: 'root' })
export class PickerService {

  constructor(public editorService: EditorService) { }

  open(config: PickerConfiguration, data?: any, editor?: Type<any>) {
    const editorConfig = {...config.editor, data: { config: config, items: data } };
    this.editorService.open(editor ?? PickerEditorComponent, editorConfig);
  }

  selected() {
    return this.editorService.onClosed();
  }
}
