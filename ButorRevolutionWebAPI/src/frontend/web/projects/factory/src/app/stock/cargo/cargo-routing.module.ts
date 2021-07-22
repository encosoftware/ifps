import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CargoPageComponent } from './pages/cargo.page.component';
import { StockCargoPageComponent } from './pages/edit-cargo/edit-cargo.page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: CargoPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetCargos]
          },
    },
    {
        path: ':id',
        component: StockCargoPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.UpdateCargos]
          },
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class StorageCargoRoutingModule { }
