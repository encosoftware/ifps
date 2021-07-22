import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { WorkstationsPageComponent } from './pages/workstations-page.component';
import { WorkstationsRoutingModule } from './workstations-routing.module';
import { workstationsReducers } from './store/reducers';
import { StoreModule } from '@ngrx/store';
import { NewWorkstationComponent } from './components/new-workstation/new-workstation.component';
import { AddCameraComponent } from './components/add-camera/add-camera.component';

@NgModule({
  declarations: [
    WorkstationsPageComponent,
    NewWorkstationComponent,
    AddCameraComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    WorkstationsRoutingModule,
    StoreModule.forFeature('workstations', workstationsReducers)
  ],
  entryComponents: [
    NewWorkstationComponent,
    AddCameraComponent
  ]
})
export class WorkstationsModule { }
