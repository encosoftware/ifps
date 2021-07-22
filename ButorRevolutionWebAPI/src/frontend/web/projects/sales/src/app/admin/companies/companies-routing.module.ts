import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CompaniesComponent } from './pages/companies.component';
import { EditCompanyComponent } from './pages/edit-company/edit-company.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
    path: '',
    component: CompaniesComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum['GetCompanies']]
    },
  },
  {
    path: 'edit/:companyId',
    component: EditCompanyComponent,
    data: {
      breadcrumb: 'Edit Company',
      claims: ClaimPolicyEnum[ClaimPolicyEnum['UpdateCompanies']]
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CompaniesRoutingModule { }
