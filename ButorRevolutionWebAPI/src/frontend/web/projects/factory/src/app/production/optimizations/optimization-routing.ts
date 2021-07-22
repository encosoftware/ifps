import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OptimizationPageComponent } from './pages/optimization-page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: OptimizationPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetOptimizations]
          },
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class OptimizationRoutingModule { }
