import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable({ providedIn: "root" })
export class LoaderService {
    
  state: BehaviorSubject<boolean> = new BehaviorSubject(false);

  show() {
    setTimeout(() => this.state.next(true), 0);
  }

  hide() {
    setTimeout(() => this.state.next(false), 0);
  }
}