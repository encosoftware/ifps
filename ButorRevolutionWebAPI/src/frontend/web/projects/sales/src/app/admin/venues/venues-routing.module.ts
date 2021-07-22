import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { VenuesComponent } from './pages/venues.component';
import { EditVenuePageComponent } from './pages/edit-venue/edit-venue.page.component';
import { ClaimPolicyEnum } from '../../shared/clients';

const routes: Routes = [
  {
    path: '',
    component: VenuesComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum['GetVenues']]
    },
  },
  {
    path: ':id',
    component: EditVenuePageComponent,
    data: {
      claims: ClaimPolicyEnum[ClaimPolicyEnum['UpdateVenues']]
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VenuesRoutingModule { }
