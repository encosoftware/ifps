import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AssemblyPageComponent } from './pages/assembly.page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
    {
        path: '',
        component: AssemblyPageComponent,
        data: {
            claims: ClaimPolicyEnum[ClaimPolicyEnum.GetAssemblies]
          },
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AssemblyRoutingModule { }
