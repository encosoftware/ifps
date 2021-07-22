import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GeneralRulesPageComponent } from './pages/general-rules.page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: GeneralRulesPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetGeneralExpenses]
          },
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class GeneralRulesRoutingModule { }
