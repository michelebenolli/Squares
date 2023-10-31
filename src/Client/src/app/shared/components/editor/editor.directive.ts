import { ComponentRef, Directive, Input, OnDestroy, OnInit, Type, ViewContainerRef } from '@angular/core';
import { EditorComponent } from './editor.component';
import { EditorBaseContent } from './models/editor-base-content';

@Directive({
  selector: '[appEditor]',
})
export class EditorDirective implements OnInit, OnDestroy {

  @Input() component!: Type<EditorBaseContent>;
  @Input() editor!: EditorComponent;
  componentRef!: ComponentRef<any>;

  constructor(private viewContainerRef: ViewContainerRef) { }

  ngOnInit() {
    this.componentRef = this.viewContainerRef.createComponent(this.component);
    this.componentRef.instance.editor = this.editor;
  }

  ngOnDestroy() {
    this.componentRef.destroy();
  }
}
