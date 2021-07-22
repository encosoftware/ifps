import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './core/components/layout/layout.component';
import { CoreModule } from './core/core.module';
import { LoginComponent } from './core/pages/login/login.component';
import { AuthGuard } from './core/guards/auth.guard';
import { ClaimGuard } from './core/guards/claim.guard';
import { LoginGuard } from './core/guards/login.guard';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: LayoutComponent,
    canActivate: [AuthGuard, ClaimGuard],
    canActivateChild: [AuthGuard, ClaimGuard],
    loadChildren: () => CoreModule
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [LoginGuard],
    // runGuardsAndResolvers: 'always',
  },
  {
    path: 'supply',
    component: LayoutComponent,
    canActivate: [AuthGuard, ClaimGuard],
    canActivateChild: [AuthGuard, ClaimGuard],
    data: {
      breadcrumb: 'Supply'
    },
    loadChildren: () => import('./supply/supply.module').then(m => m.SupplyModule)
  },
  {
    path: 'admin',
    component: LayoutComponent,
    canActivate: [AuthGuard, ClaimGuard],
    canActivateChild: [AuthGuard, ClaimGuard],
    data: {
      breadcrumb: 'admin'
    },
    loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule)
  },
  {
    path: 'stock',
    component: LayoutComponent,
    canActivate: [AuthGuard, ClaimGuard],
    canActivateChild: [AuthGuard, ClaimGuard],
    data: {
      breadcrumb: 'Stock'
    },
    loadChildren: () => import('./stock/stock.module').then(m => m.StockModule)
  },
  {
    path: 'production',
    component: LayoutComponent,
    canActivate: [AuthGuard, ClaimGuard],
    canActivateChild: [AuthGuard, ClaimGuard],
    data: {
      breadcrumb: 'Production'
    },
    loadChildren: () => import('./production/production.module').then(m => m.ProductionModule)
  },
  {
    path: 'finance',
    component: LayoutComponent,
    canActivate: [AuthGuard, ClaimGuard],
    canActivateChild: [AuthGuard, ClaimGuard],
    data: {
      breadcrumb: 'Finance'
    },
    loadChildren: () => import('./finance/finance.module').then(m => m.FinanceModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
