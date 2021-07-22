import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InspectionPageComponent } from './pages/inspection.page.component';
import { NewInspectionPageComponent } from './pages/new-inspection/new-inspection.page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: InspectionPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetInspections]
          },
    },
    {
        path: ':id/report',
        component: NewInspectionPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.UpdateInspections]
          },
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class InspectionRoutingModule { }
