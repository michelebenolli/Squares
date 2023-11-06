import { Component, EventEmitter, Injector, Input, OnInit, Output } from '@angular/core';
import { TableColumn } from './models/table-column';
import { MatTableDataSource } from '@angular/material/table';
import { PageEvent } from '@angular/material/paginator';
import { Filter } from '../filters/models/filter';
import { SortEvent } from 'primeng/api';
import { Sort } from '@angular/material/sort';
import { PagedRequest } from '../../models/paged-request';
import { FilterRequest } from '../../models/filter-request';
import { setFilters } from '../../other/utils';
import { PagedList } from '../../models/paged-list';
import { TableAction } from './models/table-action';
import { Action } from '../actions/models/action';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
})
export class TableComponent implements OnInit {

  @Input() columns!: TableColumn<any>[];
  @Input() actions?: TableAction<any>[];
  @Input() filters?: Filter[];
  @Input('pagedList') paging?: PagedList<any>;
  @Input() set data(data: any[] | undefined) { this.setDataSource(data) }
  @Input() service: any;
  @Output() onSort: EventEmitter<Sort> = new EventEmitter<Sort>();
  @Output() onAction: EventEmitter<any> = new EventEmitter();
  @Output() onPageChanged = new EventEmitter<PagedRequest>();
  @Output() onFilter = new EventEmitter<PagedRequest>();

  items?: any[];
  columnNames!: string[];
  dataSource?: any;
  selected?: any[];
  params: PagedRequest = { pageNumber: 1, pageSize: 10 }
  rowActions?: Action<any>[];
  tableActions?: Action<any>[];
  bulkActions?: Action<any>[];

  constructor(protected injector: Injector) { }

  ngOnInit() {
    if (this.service != null) this.service = this.injector.get<any>(this.service);
    this.setDataSource(this.data);

    this.rowActions = this.actions?.filter(x => x.type === 'row');
    this.tableActions = this.actions?.filter(x => x.type === 'table');
    this.bulkActions = this.actions?.filter(x => x.type === 'multiple');

    this.columnNames = this.columns.map(x => x.name);
    if (this.rowActions) this.columnNames.push('row-actions');
    setFilters(this.params, this.filters?.map(x => x.request));
  }

  getItems() {
    this.service.search(this.params).subscribe((result: any) => {
      this.dataSource = new MatTableDataSource<any>(result.data);
      this.setItems(result.data);
      this.paging = result;
    });
  }

  clearSelection() {
    this.selected = [];
  }

  setDataSource(data: any) {
    if (data) {
      this.dataSource = new MatTableDataSource<any>(data);
      this.setItems(data);
    }
    this.selected = [];
  }

  setItems(data: any[]) {
    this.items = data;
  }

  sort(event: SortEvent) {
    const active = this.columns.find(x => x.sort === event.field)?.sort;
    if (active) {
      const direction = (event.order == 1) ? 'asc' : 'desc';
      this.params.orderBy = [active + ' ' + direction];
      if (this.service) this.getItems();
      this.onSort.emit({ active, direction });
    }
  }

  onPageChange(event: PageEvent) {
    this.params.pageNumber = event.pageIndex + 1 ?? 1;
    this.params.pageSize = event.pageSize ?? 10;
    if (this.service) this.getItems();
    this.onPageChanged.emit(this.params);
  }

  filter(requests: FilterRequest[]) {
    this.params = { 
      pageNumber: 1, 
      pageSize: this.params.pageSize, 
      orderBy: this.params.orderBy 
    };
    setFilters(this.params, requests);

    if (this.service) this.getItems();
    this.onFilter.emit(this.params);
  }
}
