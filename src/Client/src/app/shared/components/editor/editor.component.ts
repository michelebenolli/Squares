import { NgbActiveOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { EditorConfig } from './models/editor-config';
import { AfterContentChecked, ChangeDetectorRef, Component, EventEmitter, InjectionToken, Input, Type } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { EditorTab } from './models/editor-tab';

// Injection token that can be used to access the editor
export const EDITOR = new InjectionToken<EditorComponent>('editor');

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.scss']
})
export class EditorComponent implements AfterContentChecked {

  @Input() component!: Type<any>;
  @Input() config!: EditorConfig;
  onSave = new EventEmitter();
  loading: boolean = false;
  tabs?: MenuItem[];
  activeTab?: MenuItem;

  constructor(
    public offcanvas: NgbActiveOffcanvas,
    private changeDetector: ChangeDetectorRef) { }

  save() {
    this.onSave.emit();
  }

  close(data?: any) {
    this.offcanvas.close(data);
  }

  ngAfterContentChecked(): void {
    this.changeDetector.detectChanges();
  }

  setTabs(tabs: EditorTab[]) {
    this.tabs = tabs.map<MenuItem>(x => ({ ...x, icon: 'bi bi-' + x.icon }));
    this.activeTab = this.tabs.find(x => x.id === this.config.activeTab) ?? this.tabs[0];
  }
}
