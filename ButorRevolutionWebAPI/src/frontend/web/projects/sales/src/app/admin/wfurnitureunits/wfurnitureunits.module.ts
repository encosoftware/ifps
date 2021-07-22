import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { WFUPageComponent } from './pages/wfurnitureunits-page.component';
import { WFURoutingModule } from './wfurnitureunits-routing.module';
// import { NewWFUComponent } from './components/new-camera/new-wfurnitureunit.component';
import { wFurnitereUnitsReducers } from './store/reducers';
import { NewWFUComponent } from './components/new-wfurnitureunit/new-wfurnitureunit.component';

@NgModule({
  declarations: [
    WFUPageComponent,
    NewWFUComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    WFURoutingModule,
    StoreModule.forFeature('wFurnitereUnits', wFurnitereUnitsReducers)
  ],
  entryComponents: [
    NewWFUComponent
  ]
})
export class WFUModule { }
