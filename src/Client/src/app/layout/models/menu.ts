import { MenuItem } from "primeng/api";

export const menu: MenuItem[] = [
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
                routerLink: ['identity/users']
            },
            {
                label: 'Ruoli',
                routerLink: ['identity/roles']
            }
        ]
    }
];