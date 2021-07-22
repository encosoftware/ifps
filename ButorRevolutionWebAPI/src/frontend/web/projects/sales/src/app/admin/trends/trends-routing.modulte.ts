import { Routes, RouterModule } from "@angular/router";
import { NgModule } from '@angular/core';
import { TrendsComponent } from './pages/trends/trends.component';

const routes: Routes = [
    {
        path: '',
        component: TrendsComponent
      }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class TrendsRoutingModule { }