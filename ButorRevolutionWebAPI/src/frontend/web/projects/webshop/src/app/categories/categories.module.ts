import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CategoriesRoutingModule } from './categories-routing.module';
import { CategoriesComponent } from './pages/categories.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [CategoriesComponent],
  imports: [
    SharedModule,
    CategoriesRoutingModule
  ]
})
export class CategoriesModule { }
