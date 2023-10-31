import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MaterialModule } from './modules/material.module';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { NgbOffcanvasModule } from '@ng-bootstrap/ng-bootstrap';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { PrimeNgModule } from './modules/primeng.module';
import { MatPaginatorIt } from './other/mat-paginator-it';
import { EDITOR, EditorComponent } from './components/editor/editor.component';
import { TranslateModule } from '@ngx-translate/core';
import { ActionsComponent } from './components/actions/actions.component';
import { FiltersComponent } from './components/filters/filters.component';
import { TableComponent } from './components/table/table.component';
import { RxReactiveFormsModule } from '@rxweb/reactive-form-validators';
import { DeleteDialogComponent } from './components/delete-dialog/delete-dialog.component';
import { PickerComponent } from './components/picker/picker/picker.component';
import { PickerEditorComponent } from './components/picker/picker-editor/picker-editor.component';
import { EditorDirective } from './components/editor/editor.directive';
import { TableValueComponent } from './components/table-value/table-value.component';
import { AutocompleteComponent } from './components/auto-complete/auto-complete.component';
import { NotFoundComponent } from './components/not-found/not-found.component';

@NgModule({
  declarations: [
    EditorComponent,
    EditorDirective,
    ActionsComponent,
    FiltersComponent,
    TableComponent,
    TableValueComponent,
    PickerComponent,
    PickerEditorComponent,
    DeleteDialogComponent,
    AutocompleteComponent,
    NotFoundComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    MaterialModule,
    NgbOffcanvasModule,
    FormsModule,
    ReactiveFormsModule,
    RxReactiveFormsModule,
    PrimeNgModule,
    DragDropModule,
    TranslateModule
  ],
  providers: [
    { provide: MatPaginatorIntl, useClass: MatPaginatorIt },
    { provide: MAT_DIALOG_DATA, useValue: {} },
    { provide: EDITOR, useValue: {} },
    { provide: MatDialogRef, useValue: {} },
    FormControl
  ],
  exports: [
    ReactiveFormsModule,
    FormsModule,
    MaterialModule,
    NgbOffcanvasModule,
    CommonModule,
    PrimeNgModule,
    TranslateModule,
    DragDropModule,
    RxReactiveFormsModule,
    TableComponent,
    PickerComponent,
    PickerEditorComponent
  ],
})
export class SharedModule { }
