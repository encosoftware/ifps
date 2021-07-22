import { NgModule } from '@angular/core';

import { CoreRoutingModule } from './core-routing.module';
import { HomeComponent } from './pages/home/home.component';
import { LayoutComponent } from './components/layout/layout.component';
import { MenuComponent } from './components/menu/menu.component';
import { SharedModule } from '../shared/shared.module';
import { ErrorPageComponent } from './pages/error-page/error-page.component';
import { ForgotPasswordComponent } from './pages/forgot-password/forgot-password.component';
import { LoginComponent } from './pages/login/login.component';
import { NewPasswordComponent } from './components/new-password/new-password.component';

@NgModule({
  declarations: [
    HomeComponent,
    LayoutComponent,
    MenuComponent,
    ErrorPageComponent,
    ForgotPasswordComponent,
    LoginComponent,
    NewPasswordComponent
  ],
  imports: [
    CoreRoutingModule,
    SharedModule
  ],
  entryComponents: [ForgotPasswordComponent, NewPasswordComponent]
})
export class CoreModule { }
