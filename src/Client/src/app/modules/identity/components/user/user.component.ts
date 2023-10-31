import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { UserEditorComponent } from '../user-editor/user-editor.component';
import { Filter } from 'src/app/shared/components/filters/models/filter';
import { PagedList } from 'src/app/shared/models/paged-list';
import { TableComponent } from 'src/app/shared/components/table/table.component';
import { TableColumn } from 'src/app/shared/components/table/models/table-column';
import { EditorService } from 'src/app/shared/components/editor/editor.service';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { filters } from '../../models/filters';
import { RoleService } from '../../services/role.service';
import { TableAction } from 'src/app/shared/components/table/models/table-action';
import { DeleteDialogComponent } from 'src/app/shared/components/delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
})
export class UserComponent implements OnInit {

  filters: Filter[] = filters;
  service = UserService;
  items?: PagedList<User>;

  @ViewChild(TableComponent) table?: TableComponent;

  columns: TableColumn<User>[] = [
    { name: 'Nome', value: x => `${x.lastName} ${x.firstName}`, sort: 'user.lastName' },
    { name: 'Email', value: x => x.email, sort: 'email' }
  ];

  actions: TableAction<User>[] = [
    { name: 'update', action: this.openEditor, icon: 'pencil', type: 'row' },
    { name: 'delete', action: this.remove, icon: 'trash', dialog: DeleteDialogComponent, inMenu: true, type: 'row' },
    { name: 'enable', action: this.toggle, icon: 'check-circle', show: x => !x.isActive, type: 'row' },
    { name: 'disable', action: this.toggle, icon: 'slash-circle', show: x => x.isActive, type: 'row' },
    { name: 'create', action: this.openEditor, type: 'table' }
  ];

  constructor(
    public userService: UserService,
    public dialog: MatDialog,
    public editorService: EditorService,
    public notification: NotificationService,
    public roleService: RoleService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe((x: any) => this.items = x.data);
  }

  openAction(event: any) {
    (this as any)[event.action](event.data);
  }

  openEditor(user?: User) {
    const config = { title: user ? 'Modifica utente' : 'Creazione utente', id: user?.id };
    this.editorService.open(UserEditorComponent, config);
    this.editorService.onClosed().subscribe(() => {
      this.table?.getItems();
    });
  }

  remove(user: User) {
    this.userService.delete(user.id).subscribe(() => {
      this.notification.success('message.deleted');
      this.table?.getItems();
    });
  }

  toggle(user: User) {
    this.userService.toggle(user.id!, !user.isActive).subscribe(() => {
      this.notification.success('message.saved');
      this.table?.getItems();
    });
  }
}
