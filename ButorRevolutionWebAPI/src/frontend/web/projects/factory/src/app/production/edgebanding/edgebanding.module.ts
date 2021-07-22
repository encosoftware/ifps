import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from '../../shared/shared.module';
import { EdgebandingRoutingModule } from './edgebanding-routing.module';
import { EdgebandingPageComponent } from './pages/edgebanding.page.component';
import { edgebandingsReducers } from './store/reducers';
@NgModule({
  declarations: [
    EdgebandingPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    EdgebandingRoutingModule,
    StoreModule.forFeature('edgebandings', edgebandingsReducers)
  ],
  entryComponents: [
  ],
  providers: [
  ]
})
export class EdgebandingModule { }
