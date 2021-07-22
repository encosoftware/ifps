import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild, Optional, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PagedData } from '../../../shared/models/paged-data.model';
import { NewOrderComponent } from '../components/new-order/new-order.component';
import { Router } from '@angular/router';
import { IOrdersListViewModel, IOrderStateListViewModel } from '../models/orders';
import { Subject } from 'rxjs';
import { Store, select } from '@ngrx/store';
import { OrdersService } from '../services/orders.service';
import { tap, debounceTime, takeUntil, switchMap, map, catchError, finalize, take } from 'rxjs/operators';
import { orderFilters } from '../store/selectors/order-list.selector';
import { ChangeFilter, DeleteFilter } from '../store/actions/order-list.actions';
import { addDays, addWeeks, addMonths, addYears, subYears, subDays, subWeeks, subMonths } from 'date-fns';
import { TranslateService } from '@ngx-translate/core';
import { DivisionTypeEnum, API_BASE_URL, ClaimPolicyEnum, OrderStateEnum } from '../../../shared/clients';
import { coreLoginT } from '../../../core/store/selectors/core.selector';

@Component({
    templateUrl: './orders.page.component.html',
    styleUrls: ['./orders.page.component.scss']
})
export class OrdersPageComponent implements OnInit, OnDestroy, AfterViewInit {

    destroy$ = new Subject();
    orderFiltersForm: FormGroup;
    isLoading = false;
    userId: string;
    divisions: DivisionTypeEnum[];
    currentDivision: DivisionTypeEnum[];
    partnerDivision: DivisionTypeEnum.Partner;
    error: string | null = null;
    columnsArray: string[] = [];
    showButtons = true;
    orderStateEnum = OrderStateEnum;

    @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

    claimPolicyEnum = ClaimPolicyEnum;
    statuses: IOrderStateListViewModel[];
    dataSource: PagedData<IOrdersListViewModel> = { items: [], pageIndex: 0, pageSize: 15 };
    futureDates = [
        { name: this.translate.instant('CustomerOrders.nextDay'), toDate: addDays(new Date(), 1).toString() },
        { name: this.translate.instant('CustomerOrders.nextWeek'), toDate: addWeeks(new Date(), 1).toString() },
        { name: this.translate.instant('CustomerOrders.nextMonth'), toDate: addMonths(new Date(), 1).toString() },
        { name: this.translate.instant('CustomerOrders.nextYear'), toDate: addYears(new Date(), 1).toString() }
    ];
    pastDates = [
        { name: this.translate.instant('CustomerOrders.lastDay'), toDate: subDays(new Date(), 1).toString() },
        { name: this.translate.instant('CustomerOrders.lastWeek'), toDate: subWeeks(new Date(), 1).toString() },
        { name: this.translate.instant('CustomerOrders.lastMonth'), toDate: subMonths(new Date(), 1).toString() },
        { name: this.translate.instant('CustomerOrders.lastYear'), toDate: subYears(new Date(), 1).toString() }
    ];

    constructor(
        public dialog: MatDialog,
        private formBuilder: FormBuilder,
        private router: Router,
        private store: Store<any>,
        private orderListService: OrdersService,
        private translate: TranslateService,
        @Optional() @Inject(API_BASE_URL) public baseUrl?: string
    ) { }

    ngAfterViewInit(): void {
        this.paginator.page.pipe(
            tap((val) => this.store.dispatch(new ChangeFilter({ pager: { pageIndex: val.pageIndex, pageSize: val.pageSize } })))
        ).subscribe();
    }

    ngOnInit() {
        this.loadData();
    }

    loadData() {
        this.orderFiltersForm = this.formBuilder.group({
            orderId: '',
            workingNr: '',
            currentStatus: undefined,
            statusDeadline: undefined,
            deadline: undefined,
            responsible: '',
            customer: '',
            sales: '',
            created: undefined
        });
        this.store.pipe(
            select(coreLoginT),
            take(1)
        ).subscribe(res => this.userId = res.UserId);

        this.orderFiltersForm.valueChanges.pipe(
            debounceTime(500),
            tap((values) => this.store.dispatch(new ChangeFilter(values))),
            takeUntil(this.destroy$)
        ).subscribe();

        this.orderListService.getOrderStates().subscribe(res => this.statuses = res);
        this.store.pipe(
            select(orderFilters),
            tap(() => this.isLoading = true),
            tap(val => this.orderFiltersForm.patchValue(val, { emitEvent: false })),
            switchMap((filter) => this.orderListService.getUserDivisions(+this.userId).pipe(
                map(res => this.divisions = res),
                switchMap((divisions) => {
                    const divEnum: DivisionTypeEnum[] = divisions.map(res => {
                        this.currentDivision = DivisionTypeEnum[DivisionTypeEnum[res]];
                        // tslint:disable-next-line: max-line-length
                        this.currentDivision.toString() === DivisionTypeEnum[DivisionTypeEnum.Partner] ? this.showButtons = true : this.showButtons = false;
                        return DivisionTypeEnum[DivisionTypeEnum[res]];
                    });
                    return this.orderListService.getOrders(divEnum, filter).pipe(
                        map(result => {
                            this.dataSource = result;
                            this.error = null;
                        }),
                        catchError(() => this.error = 'Error: could not load data'),
                        finalize(() => this.isLoading = false)
                    );
                }
                ),
            )),
            takeUntil(this.destroy$)
        ).subscribe();
    }

    get hasError(): boolean {
        return !!this.error;
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.unsubscribe();
    }

    addNewOrder(): void {
        const dialogRef = this.dialog.open(NewOrderComponent, {
            width: '52rem',
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result !== undefined) {
                this.orderListService.postOrder(result).subscribe(res => {
                    this.router.navigate(['sales/orders', res]);
                });
            }
        });
    }

    clearFilter() {
        this.store.dispatch(new DeleteFilter());
    }

    openOrder(orderId: string) {
        const stringDivisions = this.divisions.map(x => x.toString());
        if (stringDivisions.includes(DivisionTypeEnum[DivisionTypeEnum.Customer])) {
            this.router.navigate(['customers/orders/' + orderId]);
        } else {
            this.router.navigate(['sales/orders/' + orderId]);
        }
    }

    deleteOrder(id: string) {
        this.orderListService.deleteOrder(id).subscribe(() => this.loadData());
    }

    installationButton(orderId: string) {
        this.orderListService.orderInstalled(orderId).subscribe(() => this.loadData());
    }

    shippedButton(orderId: string) {
        this.orderListService.orderShipped(orderId).subscribe(() => this.loadData());
    }

    onsiteSurveyButton(orderId: string) {
        this.orderListService.orderOnSiteSurvey(orderId).subscribe(() => this.loadData());
    }

}
