import { Component, OnInit } from '@angular/core';
import { PrimeNGConfig } from 'primeng/api';
import { LayoutService } from './layout/service/app.layout.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

    constructor(
        private primengConfig: PrimeNGConfig,
        private layoutService: LayoutService) { }

    ngOnInit(): void {
        this.primengConfig.ripple = true;
        this.layoutService.config = {
            ripple: true,
            inputStyle: 'outlined',
            menuMode: 'compact',
            colorScheme: 'light',
            theme: 'blue',
            menuTheme: 'darkgray',
            scale: 14
        };
    }
}
