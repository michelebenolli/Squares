import { EventEmitter, Injectable, Injector, Type } from '@angular/core';
import { NgbOffcanvas, NgbOffcanvasRef } from '@ng-bootstrap/ng-bootstrap';
import { PickerConfiguration } from './picker-configuration';
import { PickerEditorComponent } from './picker-editor/picker-editor.component';
import { Entity } from 'src/app/shared/models/entity';
import { EditorSize } from 'src/app/shared/components/editor/models/editor-size';

@Injectable({ providedIn: 'root' })
export class PickerService<T extends Entity> {

  public offcanvas!: NgbOffcanvasRef;

  constructor(public ngbOffcanvas: NgbOffcanvas, private injector: Injector) { }

  open(config: PickerConfiguration<T>, data?: any, editor?: Type<any>) {
    var service = this.injector.get<any>(config?.service);
    this.offcanvas = this.ngbOffcanvas.open(editor ?? PickerEditorComponent<T>, {
      panelClass: 'myoffcanvas-' + (config.editor.size ?? EditorSize.Small),
      position: 'end',
      backdrop: 'static',
    });
    this.offcanvas.componentInstance.config = config;
    this.offcanvas.componentInstance.data = data;
    this.offcanvas.componentInstance.service = service;
  }

  selected(): EventEmitter<T[]> {
    return this.offcanvas.componentInstance.selected;
  }
}
