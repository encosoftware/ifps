import { NgModule } from '@angular/core';

import { UsersRoutingModule } from './users-routing.module';
import { UsersComponent } from './pages/users.component';
import { NewUserComponent } from './pages/new-user/new-user.component';
import { SharedModule } from '../../shared/shared.module';
import { AddNewUserComponent } from './components/add-new-user/add-new-user.component';
import { StoreModule } from '@ngrx/store';
import { BasicInfoComponent } from './components/basic-info/basic-info.component';
import { ClaimsComponent } from './components/claims/claims.component';
import { WorkingInfoComponent } from './components/working-info/working-info.component';
import { DaysOffComponent } from './components/days-off/days-off.component';
import { NotificationsComponent } from './components/notifications/notifications.component';
import { UsersService } from './services/users.service';
import { usersReducers } from './store/reducers';
import { CheckboxPipePipe } from './components/claims/checkbox-pipe.pipe';
import { CalendarDayComponent } from './components/days-off/components/calendar-day/calendar-day.component';
import { UpdateProfilePictureComponent } from './components/custom-image-input/update-profile-picture.component';

@NgModule({
  declarations: [
    UsersComponent,
    NewUserComponent,
    AddNewUserComponent,
    BasicInfoComponent,
    ClaimsComponent,
    WorkingInfoComponent,
    DaysOffComponent,
    NotificationsComponent,
    CheckboxPipePipe,
    CalendarDayComponent,
    UpdateProfilePictureComponent
  ],
  imports: [
    SharedModule,
    UsersRoutingModule,
    StoreModule.forFeature('users', usersReducers)
  ],
  entryComponents: [
    AddNewUserComponent,
    UpdateProfilePictureComponent
  ],
  providers: [UsersService]
})
export class UsersModule { }
