import { SelectionModel } from '@angular/cdk/collections';
import { Component, Inject, OnInit } from '@angular/core';
import { forkJoin } from 'rxjs';
import { EDITOR, EditorComponent } from 'src/app/shared/components/editor/editor.component';
import { EditorBaseContent } from 'src/app/shared/components/editor/models/editor-base-content';
import { Permission } from '../../models/permission';
import { Role } from '../../models/role';
import { RoleService } from '../../services/role.service';

export interface Group {
  resource: string;
  permissions: Permission[];
  selected: number;
}

@Component({
  selector: 'app-permission-editor',
  templateUrl: './permission-editor.component.html',
  styleUrls: ['./permission-editor.component.scss']
})
export class PermissionEditorComponent extends EditorBaseContent implements OnInit {

  selection?: SelectionModel<Permission>;
  groups?: Group[];
  data!: Role;

  constructor(
    @Inject(EDITOR) public override editor: EditorComponent,
    public roleService: RoleService) { super(); }

  ngOnInit() {
    this.data = this.editor.config.data;
    this.getPermissions();
    this.editor.onSave.subscribe(() => this.save());
  }

  toggle(group: Group, permission: Permission) {
    this.selection?.toggle(permission);
    this.updateSelected(group);
  }

  getPermissions() {
    forkJoin([
      this.roleService.getPermissions(),
      this.roleService.getPermissions(this.data.id)
    ])
    .subscribe(result => {
      const permissions = result[0].map(x => this.getPermission(x));
      const rolePermissions = result[1].map(x => this.getPermission(x));
      const resources = [...new Set(permissions.map(x => x.resource))];

      this.groups = resources.map(x => {
        return<Group> {
          resource: x,
          permissions: permissions.filter(y => y.resource == x),
          selected: rolePermissions.filter(y => y.resource == x).length
        }
      });

      const compareFn = (x: Permission, y: Permission) => x.name === y.name;
      const selected = permissions.filter(x => rolePermissions.some(y => x.name === y.name));
      this.selection = new SelectionModel<Permission>(true, selected, true, compareFn);
    });
  }

  getPermission(name: string): Permission {
    const parts = name.split('.');
    return { name: name, resource: parts[1], action: parts[2] };
  }

  save() {
    const selected = this.selection?.selected.map(x => x.name) ?? [];
    this.roleService.updatePermissions(this.data.id, selected).subscribe(() => {
      this.editor.close();
    });
  }

  updateSelected(group: Group) {
    group.selected = this.selection?.selected
      .filter(x => x.resource === group.resource).length ?? 0;
  }

  toggleAll(group: Group) {
    group.selected === group.permissions.length ?
      this.selection?.selected.filter(x => x.resource === group.resource)
        .forEach(x => this.selection?.deselect(x)) :
      group.permissions.forEach(x => this.selection?.select(x));
    this.updateSelected(group);
  }
}
