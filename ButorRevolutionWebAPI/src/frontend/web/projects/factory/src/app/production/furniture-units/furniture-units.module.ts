import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { FURoutingModule } from './furniture-units-routing.module';
import { FurnitureUnitsComponent } from './pages/furniture-units-page.component';
import { StoreModule } from '@ngrx/store';
import { FurnitureUnitsService } from './services/furniture-units.service';
import { FurnitureUnitsReducers } from './store/reducers';

@NgModule({
    declarations: [
        FurnitureUnitsComponent
    ],
    imports: [
        CommonModule,
        SharedModule,
        FURoutingModule,
        StoreModule.forFeature('FurnitureUnits', FurnitureUnitsReducers)
    ],
    providers: [
        FurnitureUnitsService
    ]
})
export class FurnitureUnitsModule { }
