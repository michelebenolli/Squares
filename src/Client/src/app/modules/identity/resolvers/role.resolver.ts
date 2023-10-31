import { inject } from "@angular/core";
import { ResolveFn, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { Role } from "../models/role";
import { RoleService } from "../services/role.service";
import { getRequest } from "src/app/shared/other/utils";
import { PagedRequest } from "src/app/shared/models/paged-request";

export const RoleResolver: ResolveFn<Role> = (
  _route: ActivatedRouteSnapshot,
  _state: RouterStateSnapshot,
  service: RoleService = inject(RoleService),
  params: PagedRequest = getRequest(),
): Observable<any> => service.search(params);
  