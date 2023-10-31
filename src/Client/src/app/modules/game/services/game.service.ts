import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { Game } from "../models/game";
import { RepositoryService } from "src/app/shared/services/repository.service";

@Injectable({ providedIn: 'root' })
export class GameService extends RepositoryService<Game> {

  constructor(http: HttpClient) {
    super(http, environment.apiUrl + 'v1/games/');
  }
}
