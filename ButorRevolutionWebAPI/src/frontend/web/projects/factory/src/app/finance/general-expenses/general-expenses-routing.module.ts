import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GeneralExpensesPageComponent } from './pages/general-expenses.page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: GeneralExpensesPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetGeneralExpenses]
          },
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class GeneralExpensesRoutingModule { }
