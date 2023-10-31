import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MenuItem } from 'primeng/api';
import { TranslateService } from '@ngx-translate/core';
import { Action } from './models/action';

@Component({
  selector: 'app-actions',
  templateUrl: './actions.component.html',
  styleUrls: ['./actions.component.scss']
})
export class ActionsComponent<T = any> implements OnInit {
  
  @Input() emitter!: EventEmitter<any>;
  @Input() actions?: Action<any>[];
  @Input() item?: T | T[];
  @Input() type: 'button' | 'icon' = 'icon';
  menuItems?: MenuItem[];

  constructor(
    public dialog: MatDialog,
    public translate: TranslateService) { }

  ngOnInit() {
    this.getMenuItems();
  }

  openAction(item: any, action: Action<any>) {
    if (action.action) {
      const event = { action: action.action.name, data: item };
      if (action.dialog) {
        this.dialog.open(action.dialog).afterClosed().subscribe((result) => {
          if (result) this.emitter.emit(event);
        });
      }
      else this.emitter.emit(event);
    }
  }

  getMenuItems() {
    const actions = this.actions?.filter(x => x.inMenu && x.show?.(this.item) !== false);
    if (actions?.length) {
      this.translate.get(actions.map(x => 'action.' + x.name)).subscribe(t => {
        this.menuItems = actions.map<MenuItem>(x => ({
          label: t['action.' + x.name],
          icon: 'bi bi-' + (x.icon ?? 'circle'),
          command: () => this.openAction(this.item, x)
        }));
      });
    }
  }
}