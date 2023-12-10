import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { Filter } from './models/filter';
import { FilterType } from './models/filter-type';
import { FilterRequest } from '../../models/filter-request';
import { hasValue } from '../../other/utils';
import { OverlayComponent } from '../overlay/overlay.component';
import * as _ from 'lodash';

@Component({
  selector: 'app-filters',
  templateUrl: './filters.component.html',
  styleUrls: ['./filters.component.scss'],
})
export class FiltersComponent implements OnInit, OnDestroy {

  @Input() filters?: Filter[];
  @Output() onFilter = new EventEmitter<FilterRequest[]>();
  @ViewChild(OverlayComponent) overlay?: OverlayComponent;
  FilterType = FilterType;
  hasFilters!: boolean;
  hasOverlayFilters!: boolean;

  ngOnInit() {
    const visible = this.filters?.filter(x => x.type != FilterType.Hidden);
    this.hasFilters = !!visible?.some(x => !x.overlay);
    this.hasOverlayFilters = !!visible?.some(x => x.overlay);
  }

  apply(filter?: Filter) {
    if (filter?.overlay) return;
    this.overlay?.hide();
    this.onFilter.emit(this.getRequests());
  }

  clear() {
    this.filters?.filter(x => x.type != FilterType.Hidden).forEach(x => x.request.value = undefined);
    this.filters = _.cloneDeep(this.filters);
    this.onFilter.emit(this.getRequests());
  }

  getRequests(): FilterRequest[] | undefined {
    return this.filters?.filter(x => hasValue(x.request)).map(x => x.request);
  }

  ngOnDestroy() {
    this.filters?.filter(x => x.type != FilterType.Hidden).forEach(x => x.request.value = undefined);
  }
}
