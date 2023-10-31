import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RxwebValidators } from '@rxweb/reactive-form-validators';
import { AccountService } from 'src/app/modules/auth/services/account.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-new-password',
	templateUrl: './new-password.component.html',
  styleUrls: ['./new-password.component.scss']
})
export class NewPasswordComponent {

  rememberMe: boolean = false;
  form!: FormGroup;
  token = '';
  tenant = 'root';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private accountService: AccountService,
    private notification: NotificationService) { }

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'];
      this.tenant = params['tenant'] ?? 'root';

      this.form = this.fb.group({
        email: [params['email'], [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', [Validators.required, RxwebValidators.compare({ fieldName: 'password' })]]
      });
    });
  }

  onSubmit() {
    this.form.markAllAsTouched();
    if (!this.form.valid) return;

    const resetPassword = {
      email: this.form.get('email')?.value,
      password: this.form.get('password')?.value,
      token: this.token
    };

    this.accountService.resetPassword(resetPassword, this.tenant).subscribe({
      next: () => this.router.navigate(['../login'], { relativeTo: this.route }).then(() => {
        this.notification.success('message.saved');
      }),
      error: () => this.notification.error('message.saveFailed')
    });
  }
}
