import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormControl } from '@angular/forms';
import { PickerConfiguration } from '../picker-configuration';
import { PickerService } from '../picker.service';
import { Entity } from 'src/app/shared/models/entity';

@Component({
  selector: 'app-picker',
  templateUrl: './picker.component.html',
  styleUrls: ['./picker.component.scss']
})
export class PickerComponent<T extends Entity> implements OnChanges {

  @Input() label?: string;
  @Input() config!: PickerConfiguration<T>;
  @Input() data?: T | T[];
  @Input() control?: FormControl;
  @Input() disabled?: boolean;
  @Output() change = new EventEmitter<number | number[] | undefined>();
  items!: T[];

  constructor(public pickerService: PickerService<T>) {  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['data']) {
      this.items = this.setValue(changes['data'].currentValue);
      this.update();
    }
  }

  openPicker() {
    this.pickerService.open(this.config, this.items);
    this.pickerService.selected().subscribe((result: T[]) => {
      this.items = result;
      this.update();
    });
  }

  remove(i: number) {
    this.items.splice(i, 1);
    this.update();
  }

  update() {
    const value = this.getValue();
    this.control?.patchValue(value);
    this.change.emit(value);
  }

  hasError(): boolean {
    return !!this.control?.hasError && !!this.control?.touched;
  }

  setValue = (x: T | T[]) => Array.isArray(x) ? x : x ? [x] : [];
  getValue = (): number | undefined | number[] => this.config.multiple ?
    this.items.map(x => x.id!) : this.items[0]?.id;
}
