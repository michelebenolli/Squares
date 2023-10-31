import { Component, EventEmitter, HostListener, Input, OnDestroy, Output } from '@angular/core';
import { Filter } from './models/filter';
import { FilterType } from './models/filter-type';
import * as moment from 'moment';
import { FilterRequest } from '../../models/filter-request';
import { format } from 'date-fns';
import { hasValue } from '../../other/utils';

@Component({
  selector: 'app-filters',
  templateUrl: './filters.component.html',
  styleUrls: ['./filters.component.scss'],
})
export class FiltersComponent implements OnDestroy {

  @Input() filters?: Filter[];
  @Output() onFilter: EventEmitter<FilterRequest[]> = new EventEmitter<FilterRequest[]>();
  filterType = FilterType;
  inputText: any;
  date: any;

  applyFilter() {
    const requests = this.filters?.filter(x => hasValue(x.request) || x.request.specificField).map(x => x.request);
    this.onFilter.emit(requests);
  }

  valueChange(event: any) {
    const filter = this.filters?.find(x => x.name === event.name);
    if (filter) filter.request.value = event.value.id ?? event.value.key;
    this.applyFilter();
  }

  clearInput(filter: Filter) { 
    const item = this.filters?.find(x => x.name === filter.name);
    if (item) item.request.value = '';
    this.applyFilter();
  }

  dateChanged(event: any, name: string) {
    if (!event.value) return;
    const filter = this.filters?.find(x => x.name === name);
    if (filter) filter.request.value = format(moment(event.value).toDate(), 'yyyy-MM-dd');
    this.applyFilter();
  }

  ngOnDestroy(): void {
    this.filters?.forEach(x => x.request.value = null);
  }

  onInputChange() {
    setTimeout(() => this.applyFilter(), 500);
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.key === 'Enter') this.applyFilter()
  }
}
