import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { MaterialPackagePageComponent } from './pages/material-packages-page.component';
import { NewMaterialPackageComponent } from './components/new-material-package/new-material-package.component';
import { MaterialPackageRoutingModule } from './material-packages-routing.module';
import { materialPackagesListReducer } from './store/reducers/material-packages-list.reducer';
import { MaterialPackageService } from './services/material-packages.service';
import { materialPackagesReducers } from './store/reducers';


@NgModule({
  declarations: [
    MaterialPackagePageComponent,
    NewMaterialPackageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    MaterialPackageRoutingModule,
    StoreModule.forFeature('materialPackages', materialPackagesReducers)
  ],
  entryComponents: [
    NewMaterialPackageComponent
  ],
  providers: [
    MaterialPackageService
  ]
})
export class MaterialPackageModule { }
