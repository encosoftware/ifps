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
    path: 'venues',
    loadChildren: () => import('./venues/venues.module').then(m => m.VenuesModule),
    data: {
      breadcrumb: 'Venues'
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
    path: 'materials',
    loadChildren: () => import('./materials/materials.module').then(m => m.MaterialsModule),
    data: {
      breadcrumb: 'Materials'
    }
  },
  {
    path: 'products',
    loadChildren: () => import('./products/products.module').then(m => m.ProductsModule),
    data: {
      breadcrumb: 'Products'
    }
  },
  {
    path: 'wfurnitureunits',
    loadChildren: () => import('./wfurnitureunits/wfurnitureunits.module').then(m => m.WFUModule),
    data: {
      breadcrumb: 'WebshopFurnitureUnits'
    }
  },
  {
    path: 'categories',
    loadChildren: () => import('./categories/categories.module').then(m => m.CategoriesModule),
    data: {
      breadcrumb: 'Categories'
    }
  },
  {
    path: 'statiscs',
    loadChildren: () => import('./statistics/statistics.module').then(m => m.StatisticsModule),
    data: {
      breadcrumb: 'Statiscs'
    }
  },
  {
    path: 'trends',
    loadChildren: () => import('./trends/trends.module').then(m => m.TrendsModule),
    data: {
      breadcrumb: 'Trends'
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
