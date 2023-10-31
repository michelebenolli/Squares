import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Role } from '../../models/role';
import { RoleService } from '../../services/role.service';
import { RoleEditorComponent } from '../role-editor/role-editor.component';
import { ActivatedRoute } from '@angular/router';
import { DeleteDialogComponent } from 'src/app/shared/components/delete-dialog/delete-dialog.component';
import { EditorService } from 'src/app/shared/components/editor/editor.service';
import { TableAction } from 'src/app/shared/components/table/models/table-action';
import { TableColumn } from 'src/app/shared/components/table/models/table-column';
import { TableComponent } from 'src/app/shared/components/table/table.component';
import { PagedList } from 'src/app/shared/models/paged-list';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { PermissionEditorComponent } from '../permission-editor/permission-editor.component';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.scss'],
})
export class RoleComponent implements OnInit {

  service = RoleService;
  items?: PagedList<Role>;
  @ViewChild(TableComponent) table?: TableComponent;

  columns: TableColumn<Role>[] = [
    { name: 'Nome', value: x => x.name },
    { name: 'Descrizione', value: x => x.description }
  ];

  actions: TableAction<Role>[] = [
    { name: 'updatePermissions', action: this.openPermissionsEditor, icon: 'shield', type: 'row' },
    { name: 'update', action: this.openEditor, icon: 'pencil', type: 'row' },
    { name: 'delete', action: this.remove, icon: 'trash', dialog: DeleteDialogComponent, type: 'row' },
    { name: 'create', action: this.openEditor, type: 'table' }
  ];

  constructor(
    public roleService: RoleService,
    public dialog: MatDialog,
    public notification: NotificationService,
    public editorService: EditorService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe((x: any) => this.items = x.data);
  }

  openAction(event: any) {
    (this as any)[event.action](event.data);
  }

  openPermissionsEditor(role: Role) {
    const config = { title: 'Modifica permessi', data: role };
    this.editorService.open(PermissionEditorComponent, config);
    this.editorService.onClosed().subscribe(() => {
      this.notification.success('message.saved');
      this.table?.getItems();
    });
  }

  openEditor(role?: Role) {
    const config = { title: role ? 'Modifica ruolo' : 'Creazione ruolo', data: role };
    this.editorService.open(RoleEditorComponent, config);
    this.editorService.onClosed().subscribe(() => {
      this.table?.getItems();
    });
  }

  remove(role: Role) {
    this.roleService.delete(role.id).subscribe(() => {
      this.notification.success('message.deleted');
      this.table?.getItems();
    });
  }
}
