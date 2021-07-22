import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';

import { RolesRoutingModule } from './roles-routing.module';
import { SaveAsComponent } from './components/save-as/save-as.component';
import { FormsModule } from '@angular/forms';
import { ClaimsComponent } from './components/claims/claims.component';
import { RolesPageComponent } from './pages/roles.page.component';
import { RolesComponent } from './components/roles/roles.component';

@NgModule({
  declarations: [
    RolesPageComponent,
    SaveAsComponent,
    ClaimsComponent,
    RolesComponent
  ],
  imports: [
    RolesRoutingModule,
    SharedModule,
    FormsModule
  ],
  entryComponents: [
    SaveAsComponent
  ]
})
export class RolesModule { }
