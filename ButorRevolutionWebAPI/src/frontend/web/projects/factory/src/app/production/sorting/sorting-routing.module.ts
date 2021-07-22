import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SortingPageComponent } from './pages/sorting.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
      path: '',
      component: SortingPageComponent,
      data: {
          claims: ClaimPolicyEnum[ClaimPolicyEnum.GetAssemblies]
        },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SortingRoutingModule { }
