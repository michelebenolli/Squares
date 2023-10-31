import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewPasswordComponent } from './new-password/new-password.component';
import { ReconfirmEmailComponent } from './reconfirm-email/reconfirm-email.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { LoginComponent } from './login/login.component';
import { AuthComponent } from './auth.component';

const routes: Routes = [
  {
    path: '',
    component: AuthComponent,
    children: [
      { path: 'login', component: LoginComponent, data: { returnUrl: window.location.pathname } },
      { path: 'forgot-password', component: ForgotPasswordComponent },
      { path: 'new-password', component: NewPasswordComponent },
      { path: 'reconfirm-email', component: ReconfirmEmailComponent }
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule { }
