import { NgModule } from '@angular/core';
import { TenantRoutingModule } from './tenant-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [],
  imports: [
    SharedModule,
    TenantRoutingModule
  ]
})
export class TenantModule { }
