import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MachinesRoutingModule } from './machines-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { machinesReducers } from './store/reducers';
import { NewCuttingMachineComponent } from './components/new-cutting_machine/new-cutting_machine.component';
import { CuttingMachinesPageComponent } from './pages/cutting/cutting_machines-page.component';
import { CncMachinesPageComponent } from './pages/cnc/cnc_machines-page.component';
import { NewCncMachineComponent } from './components/new-cnc_machine/new-cnc_machine.component';
import { EdgingMachinesPageComponent } from './pages/edging/edging_machines-page.component';
import { NewEdgingMachineComponent } from './components/new-edging_machine/new-edging_machine.component';

@NgModule({
  declarations: [
    CuttingMachinesPageComponent,
    NewCuttingMachineComponent,
    CncMachinesPageComponent,
    NewCncMachineComponent,
    EdgingMachinesPageComponent,
    NewEdgingMachineComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    MachinesRoutingModule,
    StoreModule.forFeature('cncs', machinesReducers),
    StoreModule.forFeature('cuttings', machinesReducers),
    StoreModule.forFeature('edgings', machinesReducers)
  ],
  entryComponents: [
    NewCuttingMachineComponent,
    NewCncMachineComponent,
    NewEdgingMachineComponent
  ]
})
export class MachinesModule { }
