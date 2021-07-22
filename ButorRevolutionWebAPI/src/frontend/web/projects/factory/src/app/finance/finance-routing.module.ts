import { Routes, RouterModule } from '@angular/router';

import { NgModule } from '@angular/core';

const routes: Routes = [
    {
        path: 'general-rules',
        loadChildren: () => import('./general-rules/general-rules.module').then(m => m.GeneralRulesModule)
    },
    {
        path: 'general-expenses',
        loadChildren: () => import('./general-expenses/general-expenses.module').then(m => m.GeneralExpensesModule)
    },
    {
        path: 'orders',
        loadChildren: () => import('./orders-finances/orders-finances.module').then(m => m.OrdersFinancesModule)
    },
    {
        path: 'statistics',
        loadChildren: () => import('./statistics/statistics.module').then(m => m.FinanceStatisticsModule)
    }
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class FinanceRoutingModule { }
