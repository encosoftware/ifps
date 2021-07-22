import { NgModule } from '@angular/core';

import { VenuesRoutingModule } from './venues-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { VenuesComponent } from './pages/venues.component';
import { StoreModule } from '@ngrx/store';
import { VenueBasicInfoComponent } from './components/basic-info/basic-info.component';
import { VenueMeetingRoomsComponent } from './components/meeting rooms/meeting-rooms.component';
import { NewMeetingRoomComponent } from './components/new-meeting-room/new-meeting-room.component';
import { FormsModule } from '@angular/forms';
import { EditVenuePageComponent } from './pages/edit-venue/edit-venue.page.component';
import { venuesReducers } from './store/reducers';
import { NewVenueComponent } from './components/new-venue/new-venue.component';

@NgModule({
  declarations: [
    VenuesComponent,
    VenueBasicInfoComponent,
    VenueMeetingRoomsComponent,
    NewMeetingRoomComponent,
    EditVenuePageComponent,
    NewVenueComponent
  ],
  imports: [
    SharedModule,
    VenuesRoutingModule,
    FormsModule,
    StoreModule.forFeature('venues', venuesReducers)
  ],
  entryComponents: [
    NewMeetingRoomComponent,
    NewVenueComponent
  ],
})
export class VenuesModule { }
