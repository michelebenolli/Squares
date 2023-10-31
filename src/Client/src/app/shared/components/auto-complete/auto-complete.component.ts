import { Component, EventEmitter, Input, Output, OnInit, Injector } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable, map, startWith, switchMap } from 'rxjs';
import { PagedRequest } from '../../models/paged-request';
import { RepositoryService } from '../../services/repository.service';
import { Filter } from '../filters/models/filter';
import { FilterValue } from '../filters/models/filter-value';

@Component({
  selector: 'app-auto-complete',
  templateUrl: './auto-complete.component.html',
  styleUrls: ['./auto-complete.component.scss'],
})
export class AutocompleteComponent implements OnInit {

  @Input() filter!: Filter;
  @Output() valueChange = new EventEmitter();
  service!: RepositoryService<any>;
  control = new FormControl<string>('');
  options!: Observable<any>;

  constructor(private injector: Injector) { }

  ngOnInit() {
    const service = this.filter.config?.service;
    if (!service) {
      this.control.setValue(this.filter.values?.find(x => x.id === this.filter.request.value)?.value);
      this.options = this.control.valueChanges.pipe(startWith(''), map(x => this.localFilter(x)));
    }
    else {
      this.service = this.injector.get<any>(service);
      this.getData();
    }
  }

  localFilter(value?: string | null): FilterValue[] | undefined {
    if (!value) return this.filter.values;
    return this.filter.values?.filter(x => x.value?.toLowerCase().includes(value.toLowerCase()));
  }

  serviceFilter(value: string | null) {
    let params: PagedRequest = { pageNumber: 1, pageSize: 10 };
    if (value) {
      const filterRequest = this.filter.config?.filter;
      if (filterRequest?.specificField) params[filterRequest.specificField] = value;
      if (filterRequest?.field) {
        filterRequest.value = value;
        params.filters = [filterRequest];
      }
    }

    return this.service?.search(params).pipe(map(result => {
      return this.filter.values = result.data.map(x => ({ id: x.id, value: this.filter.config?.label(x) }));
    }));
  }

  getData() {
    if (this.filter.config?.service) {
      this.options = this.control.valueChanges
        .pipe(startWith(''), switchMap(x => this.serviceFilter(x)));
    }
  }

  clearInput() {
    this.control.setValue('');
    this.valueChange.emit({ name: this.filter.name, value: '' });
  }

  onSelectionChange(event: any) {
    const selected = this.filter.values?.find(x => x.value == event.option.value);
    this.valueChange.emit({ name: this.filter.name, value: selected });
  }
}
