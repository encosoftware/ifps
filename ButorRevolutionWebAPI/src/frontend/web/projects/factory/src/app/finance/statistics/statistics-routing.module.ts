import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StatisticsPageComponent } from './pages/statistics-page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
      path: '',
      component: StatisticsPageComponent,
      data: {
        claims: ClaimPolicyEnum[ClaimPolicyEnum.GetFinanceStatistics]
      },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StatisticsRoutingModule { }
