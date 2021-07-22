import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from '../../shared/shared.module';
import { AssemblyRoutingModule } from './assembly-routing.module';
import { AssemblyPageComponent } from './pages/assembly.page.component';
import { assemblysReducers } from './store/reducers';

@NgModule({
  declarations: [
    AssemblyPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    AssemblyRoutingModule,
    StoreModule.forFeature('assemblies', assemblysReducers)
  ],
  entryComponents: [
  ],
  providers: [
  ]
})
export class AssemblyModule { }
