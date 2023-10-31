import { Component, Input } from '@angular/core';
import { DataType } from '../table/models/data-type';
import { TableColumn } from '../table/models/table-column';

@Component({
  selector: 'app-table-value',
  templateUrl: './table-value.component.html',
  styleUrls: ['./table-value.component.scss']
})
export class TableValueComponent<T> {

  @Input() item!: T;
  @Input() column!: TableColumn<T>;
  DataType = DataType;
}
