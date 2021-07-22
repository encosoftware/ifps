import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './core/components/layout/layout.component';
import { ErrorPageComponent } from './core/pages/error-page/error-page.component';
import { AuthGuard } from './core/guards/auth.guard';
import { LoginGuard } from './core/guards/login.guard';
import { CoreModule } from './core/core.module';
import { ClaimGuard } from './core/guards/claim.guard';
import { LoginComponent } from './core/pages/login/login.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: LayoutComponent,
    canActivate: [AuthGuard, ClaimGuard],
    canActivateChild: [AuthGuard, ClaimGuard],
    loadChildren: () => import('./core/core.module').then(m => m.CoreModule)
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [LoginGuard]
  },
  {
    path: 'admin',
    component: LayoutComponent,
    canActivate: [AuthGuard, ClaimGuard],
    canActivateChild: [AuthGuard, ClaimGuard],
    runGuardsAndResolvers: 'always',
    data: {
      breadcrumb: 'Admin'
    },
    loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule)
  },
  {
    path: 'sales',
    component: LayoutComponent,
    canActivate: [AuthGuard, ClaimGuard],
    canActivateChild: [AuthGuard, ClaimGuard],
    runGuardsAndResolvers: 'always',
    data: {
      breadcrumb: 'Sales'
    },
    loadChildren: () => import('./sales/sales.module').then(m => m.SalesModule)
  },
  {
    path: 'customers',
    component: LayoutComponent,
    data: {
      breadcrumb: 'Customers'
    },
    loadChildren: () => import('./customer/customer.module').then(m => m.CustomerModule)
  },
  {
    path: 'error:id',
    component: ErrorPageComponent,
  },
  {
    path: '**',
    redirectTo: '/login'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
