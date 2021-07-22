import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CategoriesRoutingModule } from './categories-routing.module';
import { CategoriesComponent } from './pages/categories.component';
import { SharedModule } from '../../shared/shared.module';
import { IsActivePipe, LessThanPipe } from './pipes/is-active.pipe';
import { NewItemComponent } from './components/new-item/new-item.component';

@NgModule({
  declarations: [CategoriesComponent, IsActivePipe, NewItemComponent, LessThanPipe],
  imports: [
    CommonModule,
    SharedModule,
    CategoriesRoutingModule
  ],
  entryComponents: [NewItemComponent],
})
export class CategoriesModule { }
