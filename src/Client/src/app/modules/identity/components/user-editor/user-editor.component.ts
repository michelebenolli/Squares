import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Role } from '../../models/role';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import * as _ from 'lodash';
import { EDITOR, EditorComponent } from 'src/app/shared/components/editor/editor.component';
import { EditorBaseContent } from 'src/app/shared/components/editor/models/editor-base-content';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { rolesPickerConfig } from 'src/app/shared/other/picker-configurations';

@Component({
  selector: 'app-user-editor',
  templateUrl: './user-editor.component.html',
  styleUrls: ['./user-editor.component.scss']
})
export class UserEditorComponent extends EditorBaseContent implements OnInit {

  form!: FormGroup;
  roles?: Role[];
  data?: User;
  rolesPickerConfig = rolesPickerConfig;

  constructor(
    @Inject(EDITOR) public override editor: EditorComponent,
    private userService: UserService,
    private notification: NotificationService,
    private fb: FormBuilder) { super() }

  ngOnInit() {
    this.getData(this.userService).subscribe(result => {
      this.data = result;
      if (this.data?.id) this.getRoles(this.data.id);
      this.initForm();
    });
    this.editor.onSave.subscribe(() => this.save());
  }

  initForm() {
    this.form = this.fb.group({
      id: this.data?.id,
      firstName: [this.data?.firstName, Validators.required],
      lastName: [this.data?.lastName, Validators.required],
      email: [this.data?.email, [Validators.required, Validators.email]],
      roles: this.data?.roles
    });
  }

  getRoles(id: number) {
    this.userService.getRoles(id).subscribe((result) => {
      this.roles = result;
    });
  }

  save() {
    this.form.markAllAsTouched();
    if (!this.form.valid) return;

    if (this.data?.id) {
      this.userService.update(this.data.id, this.form.value).subscribe(() => {
        this.notification.success('message.updated');
        this.editor.close();
      })
    }
    else {
      this.userService.create(this.form.value).subscribe(() => {
        this.notification.success('message.created');
        this.editor.close();
      })
    }
  }
}
