import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Directive({
  selector: '[permission]'
})
export class PermissionDirective implements OnInit {
  @Input() permission!: string[];

  constructor(
    private viewContainerRef: ViewContainerRef,
    private templateRef: TemplateRef<any>,
    private authService: AuthService) { }

  async ngOnInit() {
    if (this.permission[0] != undefined) {
      const isAuthorized = await this.authService.isAuthorized(this.permission);
      if (!isAuthorized) {
        this.viewContainerRef.clear();
      }
      else {
        this.viewContainerRef.createEmbeddedView(this.templateRef);
      }
    }
  }
}
