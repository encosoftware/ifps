import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './pages/home/home.component';
import { LayoutComponent } from './components/layout/layout.component';
import { MenuComponent } from './components/menu/menu.component';
import { SharedModule } from '../shared/shared.module';
import { CoreRoutingModule } from './core-routing.module';
import { LoginComponent } from './pages/login/login.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { NewPasswordComponent } from './components/new-password/new-password.component';

@NgModule({
  declarations: [HomeComponent, LayoutComponent, MenuComponent, LoginComponent, ForgotPasswordComponent, NewPasswordComponent],
  imports: [
    CommonModule,
    SharedModule,
    CoreRoutingModule
  ],
  entryComponents: [ForgotPasswordComponent, NewPasswordComponent]
})
export class CoreModule { }
