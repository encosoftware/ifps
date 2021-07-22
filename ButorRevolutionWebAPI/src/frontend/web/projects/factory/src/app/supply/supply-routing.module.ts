import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';

const routes: Routes = [
    {
        path: 'cargo',
        loadChildren: () => import('./cargo/cargo.module').then(m => m.CargoModule),
        data: {
            breadcrumb: 'Cargo'
        }
        
    },
    {
        path: 'orders',
        loadChildren: () => import('./supply-orders/supply-orders.module').then(m => m.SupplyOrdersModule),
        data: {
            breadcrumb: 'Orders'
        }
    }
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SupplyRoutingModule { }
