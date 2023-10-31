import { OnInit } from '@angular/core';
import { Component } from '@angular/core';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'
})
export class AppMenuComponent implements OnInit {

    model: any[] = [];

    ngOnInit() {
        this.model = [
            {
                label: 'Partite',
                icon: 'bi bi-controller',
                routerLink: ['games']
            },
            {
                label: 'Amministrazione',
                icon: 'bi bi-shield-lock',
                items: [
                    {
                        label: 'Utenti',
                        icon: 'bi bi-people',
                        routerLink: ['identity/users']
                    },
                    {
                        label: 'Ruoli',
                        icon: 'bi bi-shield',
                        routerLink: ['identity/roles']
                    }
                ]
            }
        ];
    }
}
