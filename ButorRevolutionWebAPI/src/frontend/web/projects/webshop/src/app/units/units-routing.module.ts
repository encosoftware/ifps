import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UnitsComponent } from './pages/units.component';

const routes: Routes = [
  {
    path: '',
    component: UnitsComponent
  },
  {
    path: ':id',
    component: UnitsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UnitsRoutingModule { }
