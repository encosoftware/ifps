import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UnitsRoutingModule } from './units-routing.module';
import { UnitsComponent } from './pages/units.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [UnitsComponent],
  imports: [
    SharedModule,
    UnitsRoutingModule
  ]
})
export class UnitsModule { }
