import { NgModule } from '@angular/core';

import { CompaniesRoutingModule } from './companies-routing.module';
import { CompaniesComponent } from './pages/companies.component';
import { SharedModule } from '../../shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { BasicInfoComponent } from './components/basic-info/basic-info.component';
import { EmployeesComponent } from './components/employees/employees.component';
import { GroupsComponent } from './components/groups/groups.component';
import { AddNewGroupComponent } from './components/add-new-group/add-new-group.component';
import { FormsModule } from '@angular/forms';
import { EditCompanyComponent } from './pages/edit-company/edit-company.component';
import { AddNewCompanyComponent } from './components/add-new-company/add-new-company.component';
import { companiesReducers } from './store/reducers';

@NgModule({
  declarations: [
    CompaniesComponent,
    BasicInfoComponent,
    EmployeesComponent,
    GroupsComponent,
    AddNewGroupComponent,
    EditCompanyComponent,
    AddNewCompanyComponent,
  ],
  imports: [
    SharedModule,
    CompaniesRoutingModule,
    FormsModule,
    StoreModule.forFeature('companies', companiesReducers)

  ],
  entryComponents: [
    AddNewGroupComponent,
    AddNewCompanyComponent
  ],
})
export class CompaniesModule { }
