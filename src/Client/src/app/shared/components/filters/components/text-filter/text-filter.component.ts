import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Filter } from '../../models/filter';

@Component({
  selector: 'app-text-filter',
  templateUrl: './text-filter.component.html',
  styleUrls: ['./text-filter.component.scss']
})
export class TextFilterComponent {

  @Input() filter!: Filter;
  @Output() changed = new EventEmitter();
  value?: string;

  clear() { 
    this.setValue(undefined);
    this.value = undefined;
  }

  onInput() {
    setTimeout(() => { this.setValue(this.value)}, 500);
  }

  setValue(value?: string) {
    this.filter.request.filters?.forEach(x => x.value = value);
    this.filter.request.value = value;
    this.changed.emit();
  }
}
