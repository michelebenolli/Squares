import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthRoutingModule } from './auth-routing.module';
import { RouterModule } from '@angular/router';
import { AuthComponent } from './auth.component';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { AccountService } from 'src/app/modules/auth/services/account.service';
import { FormsModule } from '@angular/forms';
import { LoginComponent } from './login/login.component';
import { PasswordModule } from 'primeng/password';
import { NewPasswordComponent } from './new-password/new-password.component';
import { RippleModule } from 'primeng/ripple';
import { ReconfirmEmailComponent } from './reconfirm-email/reconfirm-email.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
    declarations: [
      AuthComponent,
      ForgotPasswordComponent,
      LoginComponent,
      NewPasswordComponent,
      ReconfirmEmailComponent
    ],
    imports: [
        CommonModule,
        RouterModule,
        AuthRoutingModule,
        ButtonModule,
        InputTextModule,
        SharedModule,
        PasswordModule,
        FormsModule,
        RippleModule,
    ],
    providers: [AccountService],

})
export class AuthModule { }
