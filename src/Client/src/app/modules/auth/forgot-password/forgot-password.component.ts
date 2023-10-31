import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/modules/auth/services/account.service';
import { TenantService } from 'src/app/modules/tenant/services/tenant.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {

  form!: FormGroup;
  tenant = 'root';

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private route: ActivatedRoute,
    private router: Router,
    private notification: NotificationService,
    private tenantService: TenantService) { }

  ngOnInit() {
    this.initForm();
    this.getTenant();
  }

  initForm() {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  getTenant() {
    this.tenantService.getCurrent().subscribe((result) => {
      this.tenant = result;
    });
  }

  onSubmit() {
    this.form.markAllAsTouched();
    if (!this.form.valid) return;

    const email = this.form.get('email')?.value;
    this.accountService.forgotPassword(email, this.tenant).subscribe(() => {
      this.router.navigate(['../login'], { relativeTo: this.route }).then(() => {
        this.notification.success('message.requestSent');
      });
    })
  }
}
