import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BasketsComponent } from './pages/baskets.component';

const routes: Routes = [
  {
    path: '',
    component: BasketsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BasketRoutingModule { }
