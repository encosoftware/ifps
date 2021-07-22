import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { OrderCustomerService } from '../../services/order-customer.service';
import { map, finalize, catchError } from 'rxjs/operators';
import { forkJoin, of } from 'rxjs';
import { MessagesService } from '../../../../sales/orders/services/messages.service';
import {
  IOrderAppointmentListViewModel,
  IOrderDocumentViewModel,
  IOrderHistoryListViewModel,
  IOrderDetailsBasicInfoViewModel
} from '../../../../sales/orders/models/orders';
import { IOrderMessagesModel } from '../../../../sales/orders/models/messages.model';
import { OrdersService } from '../../../../sales/orders/services/orders.service';
import { SnackbarService } from 'butor-shared-lib';
import { DocumentStateEnum, DocumentFolderTypeEnum, OrderStateEnum } from '../../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'butor-view-order',
  templateUrl: './view-order.component.html',
  styleUrls: ['./view-order.component.scss']
})
export class ViewOrderComponent implements OnInit {
  orderId: string;
  basics: IOrderDetailsBasicInfoViewModel;
  documents: IOrderDocumentViewModel;
  orderMessages: IOrderMessagesModel;
  history: IOrderHistoryListViewModel = {
    detailed: [],
    normal: []
  };
  appointments: IOrderAppointmentListViewModel[];

  isLoading = false;

  isContractClickable: boolean;
  isContractDeclined: boolean;
  isContractApproved: boolean;

  isOfferApproved: boolean;
  isOfferDeclined: boolean;
  isOfferClickable: boolean;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private orderListService: OrderCustomerService,
    private messagesService: MessagesService,
    private orderService: OrdersService,
    private snackBar: SnackbarService,
    public translate: TranslateService
  ) { }

  ngOnInit(): void {
    this.orderId = this.route.snapshot.paramMap.get('id');
    this.getData();
  }

  openOfferForm() {
    this.router.navigate(['customers/orders', this.orderId, 'offerform']);
  }

  openContractForm() {
    this.router.navigate(['customers/orders', this.orderId, 'contractform']);
  }

  refresh() {
    this.getData();
  }

  getData() {
    this.isLoading = true;
    forkJoin([
      this.messagesService.getOrderAndUserMessages(this.orderId),
      this.orderListService.getDocuments(this.orderId),
      this.orderService.getOrderBasics(this.orderId),
      this.orderListService.getOrderTicket(this.orderId),
      this.orderService.getAppointments(this.orderId)
    ]).pipe(
      map(([messages, documents, order, history, appointments]) => {
        this.documents = documents;
        this.orderMessages = messages;
        this.basics = order;
        this.history.normal = history;
        this.appointments = appointments;
      }),
      catchError(err => {
        this.snackBar.customMessage(this.translate.instant('snackbar.error'));
        return of(this.router.navigate(['sales/orders']));
      }),
      finalize(() => {
        this.isLoading = false;
        this.isContractClickable = OrderStateEnum[this.basics.currentStatus] === OrderStateEnum.WaitingForContract
          || OrderStateEnum[this.basics.currentStatus] === OrderStateEnum.WaitingForContractFeedback;
        this.isOfferClickable = OrderStateEnum[this.basics.currentStatus] === OrderStateEnum.WaitingForOffer
          || OrderStateEnum[this.basics.currentStatus] === OrderStateEnum.WaitingForOfferFeedback;
        this.contractApproved();
        this.contractDeclined();
        this.offerApproved();
        this.offerDeclined();
      })
    ).subscribe();
  }

  private contractApproved(): boolean {
    return this.isContractApproved = this.documents.versionable.some(doc =>
      doc.status === DocumentStateEnum.Approved && doc.documentGroupDto.documentFolderType === DocumentFolderTypeEnum.ContractDocument);
  }

  private contractDeclined(): boolean {
    return this.isContractDeclined = (this.documents.versionable.some(doc =>
      doc.status === DocumentStateEnum.Declined && doc.documentGroupDto.documentFolderType === DocumentFolderTypeEnum.ContractDocument) &&
      !this.documents.versionable.some(doc => doc.status === DocumentStateEnum.Approved));
  }

  private offerApproved() {
    return this.isOfferApproved = this.documents.versionable.some(doc =>
      doc.status === DocumentStateEnum.Approved && doc.documentGroupDto.documentFolderType === DocumentFolderTypeEnum.QuotationDocument);
  }

  private offerDeclined() {
    return this.isOfferDeclined = (this.documents.versionable.some(doc =>
      doc.status === DocumentStateEnum.Declined && doc.documentGroupDto.documentFolderType === DocumentFolderTypeEnum.QuotationDocument) &&
      !this.documents.versionable.some(doc => doc.status === DocumentStateEnum.Approved));
  }
}
