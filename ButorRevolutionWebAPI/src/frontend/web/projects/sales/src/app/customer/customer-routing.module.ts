import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: 'appointments',
    loadChildren: () => import('./appointments/appointmentscustomer.module').then(m => m.AppointmentsCustomerModule)
  },
  {
    path: 'orders',
    loadChildren: () => import('./orders/orderscustomer.module').then(m => m.OrdersCustomerModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerRoutingModule { }
