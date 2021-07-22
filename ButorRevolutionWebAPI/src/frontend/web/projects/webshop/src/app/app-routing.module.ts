import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './core/components/layout/layout.component';
import { LoginGuard } from './core/guards/login.guard';
import { CheckLoginGuard } from './core/guards/check-login.guard';
import { BasketEmptyGuard } from './core/guards/basket-empty.guard';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: LayoutComponent,
    loadChildren: () => import('./core/core.module').then(m => m.CoreModule)
  },
  {
    path: 'details',
    component: LayoutComponent,
    loadChildren: () => import('./products-details/products-details.module').then(m => m.ProductsDetailsModule)
  },
  {
    path: 'categories',
    component: LayoutComponent,
    loadChildren: () => import('./categories/categories.module').then(m => m.CategoriesModule)
  },
  {
    path: 'units',
    component: LayoutComponent,
    loadChildren: () => import('./units/units.module').then(m => m.UnitsModule)
  },
  {
    path: 'basket',
    component: LayoutComponent,
    canActivate: [BasketEmptyGuard],
    canActivateChild: [BasketEmptyGuard],
    loadChildren: () => import('./basket/basket.module').then(m => m.BasketModule)
  },
  {
    path: 'login',
    component: LayoutComponent,
    canActivate: [LoginGuard],
    canActivateChild: [LoginGuard],
    loadChildren: () => import('./login/login.module').then(m => m.LoginModule)
  },
  {
    path: 'register',
    component: LayoutComponent,
    canActivate: [LoginGuard],
    canActivateChild: [LoginGuard],
    loadChildren: () => import('./register/register.module').then(m => m.RegisterModule)
  },
  {
    path: 'checkout',
    component: LayoutComponent,
    canActivate: [BasketEmptyGuard],
    canActivateChild: [BasketEmptyGuard],
    loadChildren: () => import('./checkout/checkout.module').then(m => m.CheckoutModule)
  },
  {
    path: 'purchase',
    component: LayoutComponent,
    loadChildren: () => import('./purchase/purchase.module').then(m => m.PurchaseModule)
  },
  {
    path: 'account',
    component: LayoutComponent,
    canActivate: [CheckLoginGuard],
    canActivateChild: [CheckLoginGuard],
    loadChildren: () => import('./account-history/account-history.module').then(m => m.AccountHistoryModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
