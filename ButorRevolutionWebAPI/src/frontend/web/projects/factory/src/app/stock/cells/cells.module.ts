import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CellsComponent } from './pages/cells.component';
import { SharedModule } from '../../shared/shared.module';
import { CellsRoutingModule } from './cells-routing.module';
import { NewCellComponent } from './components/new-cell/new-cell.component';
import { CellsService } from './services/cells.service';
import { StoreModule } from '@ngrx/store';
import { cellsReducers } from './store/reducers';

@NgModule({
  declarations: [
    CellsComponent,
    NewCellComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    CellsRoutingModule,
    StoreModule.forFeature('cells', cellsReducers)
  ],
  entryComponents: [
    NewCellComponent
  ],
  providers: [CellsService]
})
export class CellsModule { }
