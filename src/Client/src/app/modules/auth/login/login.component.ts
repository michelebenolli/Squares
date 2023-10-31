import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TenantService } from 'src/app/modules/tenant/services/tenant.service';
import { AuthService } from 'src/app/shared/services/auth.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  form!: FormGroup;
  loading = false;
  returnUrl = '/';
  tenant = 'root';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private tenantService: TenantService,
    private route: ActivatedRoute,
    private router: Router,
    private notification: NotificationService) { }

  ngOnInit(): void {
    // Get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] ?? '/';

    if (this.authService.isAuthenticated) {
      this.router.navigateByUrl(this.returnUrl);
    }

    this.getTenant();
    this.initForm();
  }

  initForm() {
    this.form = this.fb.group({
      email: ['michele.benolli@gmail.com', Validators.required],
      password: ['Password123!', Validators.required]
    });
  }

  getTenant() {
    this.tenantService.getCurrent().subscribe((result) => {
      this.tenant = result;
    });
  }

  submit() {
    this.form.markAllAsTouched();
    if (!this.form.valid) return;

    this.loading = true;
    this.authService.login(this.form.value, this.tenant).subscribe({
      next: () => this.router.navigateByUrl(this.returnUrl),
      error: () => {
        this.loading = false;
        this.notification.error('message.loginFailed');
      }
    });
  }
}
