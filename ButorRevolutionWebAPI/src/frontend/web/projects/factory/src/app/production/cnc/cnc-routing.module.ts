import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CncPageComponent } from './pages/cnc.page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: CncPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetCncs]
          },
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CncRoutingModule { }
