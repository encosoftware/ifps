import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: 'users',
    loadChildren: () => import('./users/users.module').then(m => m.UsersModule),
    data: {
      breadcrumb: 'Users'
    },
  },
  {
    path: 'roles',
    loadChildren: () => import('./roles/roles.module').then(m => m.RolesModule),
    data: {
      breadcrumb: 'Roles'
    }
  },
  {
    path: 'companies',
    loadChildren: () => import('./companies/companies.module').then(m => m.CompaniesModule),
    data: {
      breadcrumb: 'Companies'
    }
  },
  {
    path: 'material-packages',
    loadChildren: () => import('./material-packages/material-packages.module').then(m => m.MaterialPackageModule),
    data: {
      breadcrumb: 'Material-packages'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
