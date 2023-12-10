import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { InputTextModule } from 'primeng/inputtext';
import { SidebarModule } from 'primeng/sidebar';
import { BadgeModule } from 'primeng/badge';
import { ButtonModule } from 'primeng/button';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputSwitchModule } from 'primeng/inputswitch';
import { DropdownModule } from 'primeng/dropdown';
import { RippleModule } from 'primeng/ripple';
import { DividerModule } from 'primeng/divider';
import { DialogModule } from 'primeng/dialog';
import { MenuModule } from 'primeng/menu';
import { TooltipModule } from 'primeng/tooltip';
import { StyleClassModule } from 'primeng/styleclass';
import { AppLayoutComponent } from './app.layout.component';
import { AppSidebarComponent } from './app.sidebar.component';
import { AppTopbarComponent } from './app.topbar.component';
import { AppMenuComponent } from './app.menu.component';
import { AppMenuitemComponent } from './app.menuitem.component';
import { AppFooterComponent } from './app.footer.component';
import { SharedModule } from '../shared/shared.module';
import { LoaderComponent } from './loader/loader.component';
import { BreadcrumbModule } from 'xng-breadcrumb';

@NgModule({
    declarations: [
        AppLayoutComponent,
        AppSidebarComponent,
        AppTopbarComponent,
        AppMenuComponent,
        AppMenuitemComponent,
        AppFooterComponent,
        LoaderComponent
    ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpClientModule,
        BrowserAnimationsModule,
        InputTextModule,
        SidebarModule,
        BadgeModule,
        RadioButtonModule,
        InputSwitchModule,
        BreadcrumbModule,
        ButtonModule,
        TooltipModule,
        RippleModule,
        MenuModule,
        RouterModule,
        DropdownModule,
        DividerModule,
        DialogModule,
        StyleClassModule,
        SharedModule
    ]
})
export class AppLayoutModule { }
