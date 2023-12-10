import { Component, OnInit } from '@angular/core';
import { PrimeNGConfig } from 'primeng/api';
import { LayoutService } from './layout/service/app.layout.service';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Observable, map } from 'rxjs';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

    constructor(
        private primengConfig: PrimeNGConfig,
        private layoutService: LayoutService,
        private route: ActivatedRoute,
        private title: Title,
        private breadcrumbService: BreadcrumbService
    ) { }

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
        this.setPageTitle();
    }

    private getRouteTitle(): string | undefined {
        const title = this.route.snapshot.routeConfig?.title;
        return typeof title === 'string' ? title : undefined;
    }

    private getBreadcrumbTitle(): Observable<string> {
        return this.breadcrumbService.breadcrumbs$
            .pipe(map(x => x?.[x.length - 1]?.label as string));
    }

    setPageTitle() {
        const title = this.getRouteTitle();
        if (title) this.title.setTitle(title);
        else this.getBreadcrumbTitle()
            .subscribe(x => this.title.setTitle(x ?? 'XPL'));
    }

}
