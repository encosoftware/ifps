import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { InspectionRoutingModule } from './inspection-routing.module';
import { InspectionPageComponent } from './pages/inspection.page.component';
import { inspectionReducers } from './store/reducers';
import { NewInspectionComponent } from './components/new-inspection/new-inspection.component';
import { InspectionHeaderComponent } from './components/inspection-header/inspection-header.component';
import { InspectionDetailsComponent } from './components/inspection-details/inspection-details.component';
import { NewInspectionPageComponent } from './pages/new-inspection/new-inspection.page.component';


@NgModule({
  declarations: [
    InspectionPageComponent,
    NewInspectionComponent,
    NewInspectionPageComponent,
    InspectionHeaderComponent,
    InspectionDetailsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    InspectionRoutingModule,
    StoreModule.forFeature('inspection', inspectionReducers)
  ],
  entryComponents: [
    NewInspectionComponent
  ],
  providers: [
  ]
})
export class InspectionModule { }
