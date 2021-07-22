import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {UsersComponent} from './pages/users.component';
import {NewUserComponent} from './pages/new-user/new-user.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
    path: '',
    component: UsersComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum['GetUsers']]
    },
  },
  {
    path: ':id',
    component: NewUserComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum['UpdateUsers']]
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsersRoutingModule {
}
