import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { menu } from './models/menu';
import { AuthService } from '../shared/services/auth.service';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'
})
export class AppMenuComponent implements OnInit {

    model!: MenuItem[];

    constructor(private authService: AuthService) { }

    ngOnInit() {
        const permissions = this.authService.getPermissions;
        this.model = this.filterMenu(menu, permissions ?? []);
    }

    // Show only the allowed menu items
    filterMenu(items: MenuItem[], permissions: string[]): MenuItem[] {
        return items.reduce((result: MenuItem[], item: MenuItem) => {
            if (this.isAllowed(item, permissions)) {
                const children = item.items ? this.filterMenu(item.items, permissions) : undefined;
                result.push({ ...item, items: children });
            }
            return result;
        }, []);
    }

    // Check if the menu item is allowed
    isAllowed(item: MenuItem, permissions: string[]): boolean {
        return item.items?.length ?
            item.items.some(x => this.isAllowed(x, permissions)) :
            !item.state?.['permission'] || permissions.includes(item.state['permission'].toLowerCase());
    }
}
