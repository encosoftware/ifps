import { NgModule } from '@angular/core';

import { WorkloadRoutingModule } from './workload-routing.module';
import { WorkloadComponent } from './pages/workload.component';
import { SharedModule } from '../../shared/shared.module';
import { StatusElementComponent } from './components/status-element/status-element.component';
import { PercantagePipe } from './pipes/percantage.pipe';

@NgModule({
  declarations: [WorkloadComponent, StatusElementComponent, PercantagePipe],
  imports: [
    SharedModule,
    WorkloadRoutingModule
  ]
})
export class WorkloadModule { }
