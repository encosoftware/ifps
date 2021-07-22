import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from '../../shared/shared.module';
import { CuttingsRoutingModule } from './cuttings-routing.module';
import { CuttingsPageComponent } from './pages/cuttings.page.component';
import { cuttingsReducers } from './store/reducers';

@NgModule({
  declarations: [
    CuttingsPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    CuttingsRoutingModule,
    StoreModule.forFeature('cuttings', cuttingsReducers)
  ],
  entryComponents: [
  ],
  providers: [
  ]
})
export class CuttingsModule { }
