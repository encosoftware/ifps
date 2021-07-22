import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CargoComponent } from './pages/cargo.component';
import { ConfirmationCargoComponent } from './pages/confirmation-cargo/confirmation-cargo.component';
import { ArrivedCargoComponent } from './pages/arrived-cargo/arrived-cargo.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: CargoComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetCargos]
          },
    },
    {
        path: 'confirmation/:id',
        component: ConfirmationCargoComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.UpdateCargos]
          },
    },
    {
        path: 'arrived/:id',
        component: ArrivedCargoComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.UpdateCargos]
          },
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CargoRoutingModule { }
