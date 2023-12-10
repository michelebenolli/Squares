import { Component, EventEmitter, Injector, Input, OnInit, Output } from '@angular/core';
import { PagedRequest } from 'src/app/shared/models/paged-request';
import { RepositoryService } from 'src/app/shared/services/repository.service';
import { Filter } from '../../models/filter';
import { FilterOption } from '../../models/filter-option';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-select-filter',
  templateUrl: './select-filter.component.html',
  styleUrls: ['./select-filter.component.scss']
})
export class SelectFilterComponent implements OnInit {

  @Input() filter!: Filter;
  @Output() changed = new EventEmitter();
  service!: RepositoryService;
  options: FilterOption[] = [];
  control = new FormControl<string>('');

  constructor(private injector: Injector) { }

  ngOnInit() {
    const service = this.filter.select?.service;
    if (service) this.service = this.injector.get<any>(service);
    else {
      const option = this.filter.select!.options?.find(x => x.value === this.filter.request.value);
      this.control.setValue(option?.label ?? '');
    }
  }

  getOptions() {
    const value = this.control.value;
    if (this.service) {
      let request: PagedRequest = { pageNumber: 1, pageSize: 10 };
      const filter = this.filter.select!.request;
      if (filter?.field && value) request.filters = [{ ...filter, value }];
      this.service.search(request).subscribe(x => {
        this.options = x.data.map(x => ({ label: this.filter.select!.label!(x), value: x.id }));
      });
    }
    else this.options = this.filter.select!.options?.filter(x =>
      !value || x.label?.toLowerCase().includes(value.toLowerCase())) ?? [];
  }

  onInput() {
    setTimeout(() => this.getOptions(), 500);
  }

  clear() {
    this.options = [];
    this.filter.request.value = undefined;
    this.control.setValue('');
    this.changed.emit();
  }

  onChange(event: any) {
    const option: FilterOption = event.option.value;
    this.filter.request.value = option.value;
    this.control.setValue(option.label);
    this.changed.emit();
  }
}