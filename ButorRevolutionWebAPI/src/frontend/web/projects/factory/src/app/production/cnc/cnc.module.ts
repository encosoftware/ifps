import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from '../../shared/shared.module';
import { CncRoutingModule } from './cnc-routing.module';
import { CncPageComponent } from './pages/cnc.page.component';
import { cncsReducers } from './store/reducers';

@NgModule({
  declarations: [
    CncPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    CncRoutingModule,
    StoreModule.forFeature('cncs', cncsReducers)
  ],
  entryComponents: [
  ],
  providers: [
  ]
})
export class CncModule { }
