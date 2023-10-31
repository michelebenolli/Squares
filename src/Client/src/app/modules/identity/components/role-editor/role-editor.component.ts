import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Role } from '../../models/role';
import { RoleService } from '../../services/role.service';
import { EDITOR, EditorComponent } from 'src/app/shared/components/editor/editor.component';
import { EditorBaseContent } from 'src/app/shared/components/editor/models/editor-base-content';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-role-editor',
  templateUrl: './role-editor.component.html',
  styleUrls: ['./role-editor.component.scss']
})
export class RoleEditorComponent extends EditorBaseContent implements OnInit {

  form!: FormGroup;
  data?: Role;

  constructor(
    @Inject(EDITOR) public override editor: EditorComponent,
    private roleService: RoleService,
    private notification: NotificationService,
    private fb: FormBuilder) { super(); }

  ngOnInit() {
    this.data = this.editor.config.data;
    this.initForm();
    this.editor.onSave.subscribe(() => this.save());
  }

  initForm() {
    this.form = this.fb.group({
      id: this.data?.id,
      name: [this.data?.name, Validators.required],
      description: this.data?.description
    });
  }

  save() {
    this.form.markAllAsTouched();
    if (!this.form.valid) return;
    
    if (this.data?.id) {
      this.roleService.update(this.data.id, this.form.value).subscribe(() => {
        this.notification.success('message.updated');
        this.editor.close();
      });
    }
    else {
      this.roleService.create(this.form.value).subscribe(() => {
        this.notification.success('message.created');
        this.editor.close();
      });
    }
  }
}
