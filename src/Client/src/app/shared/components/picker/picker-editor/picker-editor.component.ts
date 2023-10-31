import { NgbActiveOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { Component, EventEmitter, HostListener, Input, OnInit, Output } from '@angular/core';
import { PickerConfiguration } from '../picker-configuration';
import { SelectionModel } from '@angular/cdk/collections';
import { PageEvent } from '@angular/material/paginator';
import { Entity } from 'src/app/shared/models/entity';
import { FilterRequest } from 'src/app/shared/models/filter-request';
import { PagedList } from 'src/app/shared/models/paged-list';
import { PagedRequest } from 'src/app/shared/models/paged-request';
import { getRequest } from 'src/app/shared/other/utils';

@Component({
  selector: 'app-picker-editor',
  templateUrl: './picker-editor.component.html',
  styleUrls: ['./picker-editor.component.scss']
})
export class PickerEditorComponent<T extends Entity> implements OnInit {

  @Input() config!: PickerConfiguration<T>;
  @Input() data?: T[];
  @Output() selected = new EventEmitter<T[]>();

  service?: any;
  items?: PagedList<T>;
  params: PagedRequest = { pageNumber: 1, pageSize: 10 };
  selection!: SelectionModel<T>;

  constructor(public offcanvas: NgbActiveOffcanvas) { }

  ngOnInit(): void {
    this.selection = new SelectionModel<T>(true, this.config.multiple ? this.data : []);
    this.selection.isSelected = this.isChecked.bind(this);
    if (this.config?.filters) this.params = getRequest(this.config?.filters)
    this.getData();
  }

  getData(): void {
    this.service.search(this.params).subscribe((result: PagedList<T>) => {
      this.items = result;
    });
  }

  toggle(item: T) {
    const selected = this.selection.selected.find(x => x.id === item.id);
    if (selected) this.selection.deselect(selected);
    else this.selection.select(item);
    if (!this.config.multiple) this.save();
  }

  save() {
    this.selected.emit(this.selection.selected);
    this.offcanvas.close();
  }

  isChecked(item: T): boolean {
    return this.selection.selected.some(x => x.id === item.id);
  }

  onPageChange(event: PageEvent) {
    this.params.pageNumber = event.pageIndex + 1;
    this.getData();
  }

  filter(filters: FilterRequest[]) {
    const filter = filters.find(x => x.specificField);
    if (filter?.specificField) {
      const { specificField, value } = filter;
      this.params = { pageNumber: 1, pageSize: 10, [specificField]: value }
      filters = filters.filter(x => !x.specificField);
    }

    this.params.filters = filters;
    this.params.pageNumber = 1;
    this.getData();
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.key === 'Enter') this.save();
  }
}
