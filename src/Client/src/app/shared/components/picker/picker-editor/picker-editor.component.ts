import { Component, Inject, Injector, OnInit } from '@angular/core';
import { PickerConfiguration } from '../picker-configuration';
import { PagedList } from 'src/app/shared/models/paged-list';
import { SelectionModel } from '@angular/cdk/collections';
import { PagedRequest } from 'src/app/shared/models/paged-request';
import { Entity } from 'src/app/shared/models/entity';
import { getRequest } from '../../../other/utils';
import { EDITOR, EditorComponent } from '../../editor/editor.component';
import { PageEvent } from '@angular/material/paginator';
import { FilterRequest } from 'src/app/shared/models/filter-request';

@Component({
  selector: 'app-picker-editor',
  templateUrl: './picker-editor.component.html',
  styleUrls: ['./picker-editor.component.scss']
})
export class PickerEditorComponent<T extends Entity> implements OnInit{

  config!: PickerConfiguration<T>;
  service?: any;
  items?: PagedList<T>;
  params!: PagedRequest;
  selection!: SelectionModel<T>;

  constructor(
    @Inject(EDITOR) public editor: EditorComponent,
    private injector: Injector) { }

  ngOnInit(): void {
    const data = this.editor.config.data;
    this.config = data.config;
    this.service = this.injector.get<any>(data.config.service);
    this.editor.onSave.subscribe(() => this.save());

    this.selection = new SelectionModel<T>(true, this.config.multiple ? data.items : []);
    this.selection.isSelected = this.isChecked.bind(this);
    this.params = getRequest(this.config?.filters);
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
    this.editor.close(this.selection.selected);
  }

  isChecked(item: T): boolean {
    return this.selection.selected.some(x => x.id === item.id);
  }

  onPageChange(event: PageEvent) {
    this.params.pageNumber = event.pageIndex + 1;
    this.getData();
  }

  filter(filters: FilterRequest[]) {
    this.params.filters = filters;
    this.params.pageNumber = 1;
    this.getData();
  }
}
