import { Component } from "@angular/core";
import { LoaderService } from "src/app/shared/services/loader.service";

@Component({
  selector: "app-loader",
  templateUrl: "./loader.component.html" ,
  styleUrls: ["./loader.component.scss"]
})
export class LoaderComponent {
  show?: boolean;

  constructor(private loaderService: LoaderService) { }

  ngOnInit() {
    this.loaderService.state.subscribe(result => this.show = result);
  }
}