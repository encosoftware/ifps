import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CellsComponent } from './pages/cells.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: CellsComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetStorageCells]
          },
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CellsRoutingModule { }
