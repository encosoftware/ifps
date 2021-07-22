import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StockedItemsPageComponent } from './pages/stocked-items.page.component';
import { ReserveStockedItemsPageComponent } from './pages/reserve-stocked-items/reserve-stocked-items.page.component';
import { EjectStockedItemsPageComponent } from './pages/eject-stocked-items/eject-stocked-items.page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: StockedItemsPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetStocks]
          },
    },
    {
        path: 'reserve',
        component: ReserveStockedItemsPageComponent,
        data: {
            breadcrumb: 'Reserve',
            claims: ClaimPolicyEnum[ClaimPolicyEnum.UpdateStocks]

        }
    },
    {
        path: 'eject',
        component: EjectStockedItemsPageComponent,
        data: {
            breadcrumb: 'Eject',
            claims: ClaimPolicyEnum[ClaimPolicyEnum.UpdateStocks]

        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class StockedItemRoutingModule { }
