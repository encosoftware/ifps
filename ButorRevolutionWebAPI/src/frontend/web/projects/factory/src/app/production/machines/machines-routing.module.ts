import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ClaimPolicyEnum } from '../../shared/clients';
import { CuttingMachinesPageComponent } from './pages/cutting/cutting_machines-page.component';
import { EdgingMachinesPageComponent } from './pages/edging/edging_machines-page.component';
import { CncMachinesPageComponent } from './pages/cnc/cnc_machines-page.component';

const routes: Routes = [
    {
        path: 'cuttings',
        component: CuttingMachinesPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetMachines]
          },
    },
    {
        path: 'cncs',
        component: CncMachinesPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetMachines]
          },
    },
    {
        path: 'edgings',
        component: EdgingMachinesPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetMachines]
          },
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class MachinesRoutingModule { }
