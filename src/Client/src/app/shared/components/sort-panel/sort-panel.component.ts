import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { SortOption } from './models/sort-option';
import * as _ from 'lodash';
import { OverlayComponent } from '../overlay/overlay.component';

@Component({
  selector: 'app-sort-panel',
  templateUrl: './sort-panel.component.html',
  styleUrls: ['./sort-panel.component.scss']
})
export class SortPanelComponent {

  @Input() values!: SortOption[];
  @Input() title?: string;
  @Input() multiple?: boolean;
  @Output() changed = new EventEmitter<SortOption[]>();
  @ViewChild(OverlayComponent) overlay!: OverlayComponent;
  items: SortOption[] = [];
  selected: string[] = [];

  initialize() {
    this.items = _.cloneDeep(this.values);
    this.selected = this.items.filter(x => x.selected).map(x => x.name);
  }

  apply() {
    this.items.forEach(x => x.selected = this.selected.includes(x.name));
    this.changed.emit(this.items);
    this.overlay.hide();
  }

  drop(event: CdkDragDrop<SortOption[]>) {
    moveItemInArray(this.items, event.previousIndex, event.currentIndex);
  }

  toggleDirection(sortItem: SortOption) {
    const item = this.items.find(x => x.name === sortItem.name);
    if (item) item.direction = item?.direction === 'desc' ? 'asc' : 'desc';
  }
}
