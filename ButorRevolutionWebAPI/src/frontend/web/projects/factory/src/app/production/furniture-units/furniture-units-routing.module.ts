import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FurnitureUnitsComponent } from './pages/furniture-units-page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: FurnitureUnitsComponent,
        data: {
            //      claims: ClaimPolicyEnum[ClaimPolicyEnum.GetFurnitureUnits]
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class FURoutingModule { }
