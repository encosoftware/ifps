import { Component, OnInit } from '@angular/core';
import { OrdersService } from '../../services/orders.service';
import {
    IOrderDetailsBasicInfoViewModel,
    IOrderDocumentViewModel,
    IOrderAppointmentListViewModel,
    IOrderHistoryListViewModel,
} from '../../models/orders';
import { Router, ActivatedRoute } from '@angular/router';
import { MessagesService } from '../../services/messages.service';
import { forkJoin, of } from 'rxjs';
import { map, finalize, catchError, take } from 'rxjs/operators';
import { IOrderMessagesModel } from '../../models/messages.model';
import { FormControl } from '@angular/forms';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../../../core/store/selectors/core.selector';
import { SnackbarService } from 'butor-shared-lib';
import { OrderStateEnum, DocumentStateEnum, DocumentFolderTypeEnum, DivisionTypeEnum } from '../../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';

@Component({
    templateUrl: './edit-order.page.component.html',
    styleUrls: ['./edit-order.page.component.scss']
})
export class EditOrderPageComponent implements OnInit {

    orderBasics: IOrderDetailsBasicInfoViewModel;
    orderMessages: IOrderMessagesModel;
    orderDocuments: IOrderDocumentViewModel;
    orderAppointmnets: IOrderAppointmentListViewModel[];
    orderTickets: IOrderHistoryListViewModel = {
        detailed: [],
        normal: []
    };
    isLoading = false;
    orderId: string;
    selected = new FormControl(0);
    userId: string;

    isContractClickable: boolean;
    isContractDeclined: boolean;
    isContractApproved: boolean;

    isOfferApproved: boolean;
    isOfferDeclined: boolean;
    isOfferClickable: boolean;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private orderService: OrdersService,
        private messagesService: MessagesService,
        private store: Store<any>,
        private snackBar: SnackbarService,
        private translate: TranslateService
    ) { }

    ngOnInit(): void {
        this.orderId = this.route.snapshot.paramMap.get('id');
        this.store.pipe(
            select(coreLoginT),
            take(1)
        ).subscribe(res => this.userId = res.UserId);
        this.loadData();
    }

    loadData() {
        this.isLoading = true;
        this.orderId = this.route.snapshot.paramMap.get('id');
        forkJoin([
            this.orderService.getOrderBasics(this.orderId),
            this.messagesService.getOrderAndUserMessages(this.orderId),
            this.orderService.getOrderDocuments(this.orderId),
            this.orderService.getAppointments(this.orderId),
            this.orderService.getOrderTicket(this.orderId)
        ]).pipe(
            map(([first, second, third, fourth, fifth]) => {
                this.orderBasics = first;
                this.orderId = first.orderId;
                this.orderMessages = second;
                this.orderDocuments = third;
                this.orderAppointmnets = fourth.sort((a, b) => (a.date > b.date) ? 1 : ((b.date > a.date) ? -1 : 0));
                this.orderTickets.normal = fifth;
            }),
            catchError(err => {
                this.snackBar.customMessage(this.translate.instant('snackbar.error'));
                return of(this.router.navigate(['sales/orders']));
            }),
            finalize(() => {
                this.isLoading = false;
                this.isContractClickable =
                    this.orderBasics.currentStatusState.toString() == OrderStateEnum[OrderStateEnum.WaitingForContract] ||
                    this.orderBasics.currentStatusState.toString() == OrderStateEnum[OrderStateEnum.WaitingForContractFeedback] ||
                    this.orderBasics.currentStatusState.toString() == OrderStateEnum[OrderStateEnum.ContractSigned] ||
                    this.orderBasics.currentStatusState.toString() == OrderStateEnum[OrderStateEnum.ContractDeclined];
                if (this.orderBasics.divisions.includes(DivisionTypeEnum[DivisionTypeEnum.Partner])) {
                    this.isContractClickable = false;
                }
                this.isOfferClickable =
                    this.orderBasics.currentStatusState.toString() == OrderStateEnum[OrderStateEnum.None] ||
                    this.orderBasics.currentStatusState.toString() == OrderStateEnum[OrderStateEnum.OrderCreated] ||
                    this.orderBasics.currentStatusState.toString() == OrderStateEnum[OrderStateEnum.WaitingForOffer] ||
                    this.orderBasics.currentStatusState.toString() == OrderStateEnum[OrderStateEnum.WaitingForOfferFeedback] ||
                    this.orderBasics.currentStatusState.toString() == OrderStateEnum[OrderStateEnum.OfferDeclined];
                    if (this.orderBasics.divisions.includes(DivisionTypeEnum[DivisionTypeEnum.Partner])) {
                        this.isOfferClickable = false;
                    }
                this.contractApproved();
                this.contractDeclined();
                this.offerApproved();
                this.offerDeclined();
            })
        ).subscribe();
    }

    onSaved(saved: boolean) {
        this.loadData();
    }

    openOfferForm() {
        this.router.navigate(['sales/orders', this.orderId, 'offerform']);
    }

    openContractForm() {
        this.router.navigate(['sales/orders', this.orderId, 'contractform']);
    }

    private contractApproved(): boolean {
        return this.isContractApproved = this.orderDocuments.versionable.some(doc =>
            doc.status === DocumentStateEnum.Approved && doc.documentGroupDto.documentFolderType === DocumentFolderTypeEnum.ContractDocument);
    }

    private contractDeclined(): boolean {
        return this.isContractDeclined = (this.orderDocuments.versionable.some(doc =>
            doc.status === DocumentStateEnum.Declined && doc.documentGroupDto.documentFolderType === DocumentFolderTypeEnum.ContractDocument) &&
            !this.orderDocuments.versionable.some(doc => doc.status === DocumentStateEnum.Approved));
    }

    private offerApproved() {
        return this.isOfferApproved = this.orderDocuments.versionable.some(doc =>
            doc.status === DocumentStateEnum.Approved && doc.documentGroupDto.documentFolderType === DocumentFolderTypeEnum.QuotationDocument);
    }

    private offerDeclined() {
        return this.isOfferDeclined = (this.orderDocuments.versionable.some(doc =>
            doc.status === DocumentStateEnum.Declined && doc.documentGroupDto.documentFolderType === DocumentFolderTypeEnum.QuotationDocument) &&
            !this.orderDocuments.versionable.some(doc => doc.status === DocumentStateEnum.Approved));
    }
}
