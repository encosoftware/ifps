import { NgModule } from '@angular/core';

import { MaterialsRoutingModule } from './materials-routing.module';
import { FoilsComponent } from './pages/foils/foils.component';
import { AccessoriesComponent } from './pages/accessories/accessories.component';
import { SharedModule } from '../../shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { AddNewAccessoryComponent } from './components/accessories/new-accessory.component';
import { AddNewWorktopComponent } from './components/worktops/new-worktop.component';
import { AddNewFoilComponent } from './components/foils/new-foil.component';
import { AddNewDecoroardComponent } from './components/decorboards/new-decorboard.component';
import { DecorboardsComponent } from './pages/decorboards/decorboards.component';
import { WorktopsComponent } from './pages/worktops/worktops.component';
import { materialsReducers } from './store/reducers';
import { AppliancesComponent } from './pages/appliances/appliances.component';
import { AddNewApplianceComponent } from './components/appliances/new-appliance.component';


@NgModule({
  declarations: [
    WorktopsComponent,
    AddNewDecoroardComponent,
    FoilsComponent,
    AccessoriesComponent,
    AppliancesComponent,
    AddNewAccessoryComponent,
    AddNewApplianceComponent,
    AddNewWorktopComponent,
    AddNewFoilComponent,
    DecorboardsComponent
  ],
  imports: [
    MaterialsRoutingModule,
    SharedModule,
    StoreModule.forFeature('worktops', materialsReducers),
    StoreModule.forFeature('decorboards', materialsReducers),
    StoreModule.forFeature('foils', materialsReducers),
    StoreModule.forFeature('accessories', materialsReducers),
    StoreModule.forFeature('appliances', materialsReducers)
  ],
  entryComponents: [
    AddNewAccessoryComponent,
    AddNewApplianceComponent,
    AddNewWorktopComponent,
    AddNewFoilComponent,
    AddNewDecoroardComponent
  ]
})
export class MaterialsModule { }
