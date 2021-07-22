import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StoragesComponent as StoragesComponent } from './pages/stocks.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: StoragesComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetStorages]
          },
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class StocksRoutingModule { }
