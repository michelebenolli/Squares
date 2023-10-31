import { Injectable, Type } from '@angular/core';
import { EditorComponent } from './editor.component';
import { EditorConfig } from './models/editor-config';
import { EditorSize } from './models/editor-size';
import { NgbOffcanvas, NgbOffcanvasRef } from '@ng-bootstrap/ng-bootstrap';

@Injectable({ providedIn: 'root' })
export class EditorService {
  public offcanvas!: NgbOffcanvasRef;
  constructor(public ngOffcanvas: NgbOffcanvas) {}

  // Open the editor component
  open(component: Type<any>, config: EditorConfig) {
    this.offcanvas = this.ngOffcanvas.open(EditorComponent, {
      panelClass: 'myoffcanvas-' + (config.size ?? EditorSize.Small),
      position: 'end',
      backdrop: 'static',
    });

    this.offcanvas.componentInstance.config = config;
    this.offcanvas.componentInstance.component = component;
  }

  onClosed() {
    return this.offcanvas.closed;
  }
}
