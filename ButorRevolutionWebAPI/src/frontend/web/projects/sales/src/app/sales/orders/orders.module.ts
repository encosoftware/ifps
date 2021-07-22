import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';
import { OrdersRoutingModule } from './orders-routing.module';
import { OrdersPageComponent } from './pages/orders.page.component';
import { FormsModule, FormGroupDirective } from '@angular/forms';
import { NewOrderComponent } from './components/new-order/new-order.component';
import { EditOrderPageComponent } from './pages/edit-order/edit-order.page.component';
import { StoreModule } from '@ngrx/store';
import { ordersReducers } from './store/reducers';
import { EditOrderComponent } from './components/edit-order/edit-order.component';
import { OrderHeaderComponent } from './components/order-header/order-header.component';
import { OrderDocumentsComponent } from './components/order-documents/order-documents.component';
import { OrderDocumentUploadComponent } from './components/upload-order-document/upload-order-document.component';
import { OrderAppointmentsComponent } from './components/order-appointments/order-appointments.component';
import { OrderHistoryComponent } from './components/order-history/order-history.component';
import { OrderMessagesComponent } from './components/order-messages/order-messages.component';
import { OfferFormPageComponent } from './pages/offer-form/offer-form.page.component';
import { OrderOfferFormGeneralComponent } from './components/offer-form/general-information/general-information.component';
import { OrderOfferFormProductsComponent } from './components/offer-form/products/offer-form-products.component';
import { ContractFormPageComponent } from './pages/contract-form/contract-form.page.component';
import { OrderContractFormComponent } from './components/contract-form/contract-form.component';
import { OfferPreviwComponent } from './components/offer-preview/offer-preview.component';
import { WallCabinetDialogComponent } from './components/offer-form/edit-wall-cabinet/edit-wall-cabinet.component';
import { NewProductDialogComponent } from './components/offer-form/new-product/new-product.component';
import { EditApplianceComponent } from './components/offer-form/edit-appliance/edit-appliance.component';
import { OrderNewAppointmentComponent } from './components/new-appointment/order-new-appointment.component';
import { ContractPreviewComponent } from './components/contract-preview/contract-preview.component';
import { ContractHtmlComponent } from './components/contract-html/contract-html.component';

@NgModule({
  imports: [
    OrdersRoutingModule,
    SharedModule,
    FormsModule,
    StoreModule.forFeature('orders', ordersReducers)
  ],
  declarations: [
    OrdersPageComponent,
    NewOrderComponent,
    EditOrderPageComponent,
    EditOrderComponent,
    OrderHeaderComponent,
    OrderDocumentsComponent,
    OrderDocumentUploadComponent,
    OrderAppointmentsComponent,
    OrderHistoryComponent,
    OrderMessagesComponent,
    OfferFormPageComponent,
    OrderOfferFormGeneralComponent,
    OrderOfferFormProductsComponent,
    ContractFormPageComponent,
    OrderContractFormComponent,
    OfferPreviwComponent,
    ContractPreviewComponent,
    WallCabinetDialogComponent,
    NewProductDialogComponent,
    EditApplianceComponent,
    OrderNewAppointmentComponent,
    ContractHtmlComponent
  ],
  providers: [
    FormGroupDirective
  ],
  entryComponents: [
    NewOrderComponent,
    EditOrderComponent,
    OrderDocumentUploadComponent,
    OfferPreviwComponent,
    ContractPreviewComponent,
    WallCabinetDialogComponent,
    NewProductDialogComponent,
    EditApplianceComponent,
    OrderNewAppointmentComponent,
  ]
})
export class OrdersModule { }
