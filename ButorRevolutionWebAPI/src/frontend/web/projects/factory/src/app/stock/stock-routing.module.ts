import { Routes, RouterModule } from '@angular/router';

import { NgModule } from '@angular/core';

const routes: Routes = [
    {
        path: 'stocks',
        loadChildren: () => import('./stocks/stocks.module').then(m => m.StocksModule),
        data: {
            breadcrumb: 'Stocks'
        }
    },
    {
        path: 'cells',
        loadChildren: () => import('./cells/cells.module').then(m => m.CellsModule),
        data: {
            breadcrumb: 'Cells'
        }
    },
    {
        path: 'stockeditems',
        loadChildren: () => import('./stocked-items/stocked-items.module').then(m => m.StockedItemsModule),
        data: {
            breadcrumb: 'Stocked items'
        }
    },
    {
        path: 'cargo',
        loadChildren: () => import('./cargo/storage-cargo.module').then(m => m.StorageCargo),
        data: {
            breadcrumb: 'Cargo'
        }
    },
    {
        path: 'inspection',
        loadChildren: () => import('./inspection/inspection.module').then(m => m.InspectionModule)
    },
    {
        path: 'statistics',
        loadChildren: () => import('./statistics/stock-statistics.module').then(m => m.StockStatisticsModule)
    }
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class StockRoutingModule { }
