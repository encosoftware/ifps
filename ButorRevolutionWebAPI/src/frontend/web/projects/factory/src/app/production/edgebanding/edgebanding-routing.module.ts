import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EdgebandingPageComponent } from './pages/edgebanding.page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: EdgebandingPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetEdgeBandings]
          },
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class EdgebandingRoutingModule { }
