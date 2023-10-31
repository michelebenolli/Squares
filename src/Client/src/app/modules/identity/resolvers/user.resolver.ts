import { inject } from "@angular/core";
import { ResolveFn, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { User } from "../models/user";
import { UserService } from "../services/user.service";
import { filters } from "../models/filters";
import { PagedRequest } from "src/app/shared/models/paged-request";
import { getRequest } from "src/app/shared/other/utils";

export const UserResolver: ResolveFn<User> = (
  _route: ActivatedRouteSnapshot,
  _state: RouterStateSnapshot,
  service: UserService = inject(UserService),
  params: PagedRequest = getRequest(filters),
): Observable<any> =>  service.search(params);
