import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpResponse } from "@angular/common/http";
import { finalize, map } from "rxjs/operators";
import { LoaderService } from "../services/loader.service";

@Injectable()
export class LoaderInterceptor implements HttpInterceptor {

  constructor(private loaderService: LoaderService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler) {
    this.loaderService.show();
    return next.handle(request).pipe(
      map((request: any) => {
        if (request instanceof HttpResponse) this.loaderService.hide();
        return request;
      }),
      finalize(() => this.loaderService.hide())
    );
  }
}
