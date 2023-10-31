import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoleComponent } from './components/role/role.component';
import { UserComponent } from './components/user/user.component';
import { UserResolver } from './resolvers/user.resolver';
import { RoleResolver } from './resolvers/role.resolver';
import { PermissionGuard } from 'src/app/shared/guards/permission.guard';

const routes: Routes = [
  {
    path: 'users',
    component: UserComponent,
    resolve: { data: UserResolver },
    canActivate: [PermissionGuard],
    data: {
      permission: ['users.view'],
      breadcrumb: 'Utenti'
    }
  },
  {
    path: 'roles',
    component: RoleComponent,
    canActivate: [PermissionGuard],
    resolve: { data: RoleResolver },
    data: {
      permission: ['roles.view'],
      breadcrumb: 'Ruoli'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IdentityRoutingModule { }
