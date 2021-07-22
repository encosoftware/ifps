import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { camerasReducers } from './store/reducers';
import { StoreModule } from '@ngrx/store';
import { CamerasPageComponent } from './pages/cameras-page.component';
import { NewCameraComponent } from './components/new-camera/new-camera.component';
import { CamerasRoutingModule } from './cameras-routing.module';

@NgModule({
  declarations: [
    CamerasPageComponent,
    NewCameraComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    CamerasRoutingModule,
    StoreModule.forFeature('cameras', camerasReducers)
  ],
  entryComponents: [
    NewCameraComponent
  ]
})
export class CamerasModule { }
