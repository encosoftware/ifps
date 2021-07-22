import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CuttingsPageComponent } from './pages/cuttings.page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: CuttingsPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetCuttings]
          },
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CuttingsRoutingModule { }
