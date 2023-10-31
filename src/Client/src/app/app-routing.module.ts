import { NgModule } from '@angular/core';
import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { AppLayoutComponent } from './layout/app.layout.component';
import { AuthGuard } from './shared/guards/auth.guard';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';

const routerOptions: ExtraOptions = {
    anchorScrolling: 'enabled'
};

const routes: Routes = [
    {
        path: '', 
        component: AppLayoutComponent,
        canActivate: [AuthGuard],
        children: [
            { path: '', redirectTo: '/games', pathMatch: 'full' },
            { path: 'games', data: { breadcrumb: 'Partite' }, loadChildren: () => import('./modules/game/game.module').then(m => m.GameModule) },
            { path: 'identity', data: { breadcrumb: 'Admin' }, loadChildren: () => import('./modules/identity/identity.module').then(m => m.IdentityModule) }
        ]
    },
    { path: '', loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule) },
    { path: 'not-found', component: NotFoundComponent },
    { path: '**', redirectTo: '/not-found' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, routerOptions)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
