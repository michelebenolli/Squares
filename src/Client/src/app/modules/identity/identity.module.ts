import { NgModule } from '@angular/core';
import { IdentityRoutingModule } from './identity-routing.module';
import { TranslateModule } from '@ngx-translate/core';
import { UserComponent } from './components/user/user.component';
import { RoleComponent } from './components/role/role.component';
import { RoleEditorComponent } from './components/role-editor/role-editor.component';
import { UserService } from './services/user.service';
import { RoleService } from './services/role.service';
import { PermissionEditorComponent } from './components/permission-editor/permission-editor.component';
import { UserEditorComponent } from './components/user-editor/user-editor.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    UserComponent,
    RoleComponent,
    UserEditorComponent,
    RoleEditorComponent,
    PermissionEditorComponent
  ],
  imports: [
    IdentityRoutingModule,
    SharedModule,
    TranslateModule
  ],
  providers:[
    UserService,
    RoleService
  ]
})
export class IdentityModule { }
