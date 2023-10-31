import { inject } from "@angular/core";
import { ResolveFn, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { Observable} from "rxjs";
import { Game } from "../models/game";
import { GameService } from "../services/game.service";
import { PagedRequest } from "src/app/shared/models/paged-request";
import { getRequest } from "src/app/shared/other/utils";

export const GameResolver: ResolveFn<Game> = (
  _route: ActivatedRouteSnapshot,
  _state: RouterStateSnapshot,
  service: GameService = inject(GameService),
  params: PagedRequest = getRequest(),
): Observable<any> => service.search(params);
