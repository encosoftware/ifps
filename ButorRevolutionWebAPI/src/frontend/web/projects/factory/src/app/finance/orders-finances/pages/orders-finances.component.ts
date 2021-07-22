import { Component, OnInit, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { PagedData } from '../../../shared/models/paged-data.model';
import { IOrderStateListViewModel, IOrdersListViewModel } from '../models/customerOrders';
import { Store, select } from '@ngrx/store';
import { tap, debounceTime, takeUntil, switchMap, map, catchError, finalize, take } from 'rxjs/operators';
import { orderCustomerFilters } from '../store/selectors/order-list.selector';
import { ChangeFilter, DeleteFilter } from '../store/actions/order-list.action';
import { OrderFinancesService } from '../services/order-customer.service';
import { coreLoginT } from '../../../core/store/selectors/core.selector';
import { addDays, addWeeks, addMonths, addYears } from 'date-fns';
import { OrderSchedulingService } from '../../../production/order-scheduling/services/order-scheduling.service';
import { IOrderSchedulingStatusListViewModel } from '../../../production/order-scheduling/models/order-scheduling.model';

@Component({
    selector: 'factory-orders-finances',
    templateUrl: './orders-finances.component.html',
    styleUrls: ['./orders-finances.component.scss']
})
export class OrdersFinancesComponent implements OnInit {
    destroy$ = new Subject();

    orderFiltersForm: FormGroup;
    isLoading = false;
    error: string | null = null;
    statusesDropDown: IOrderSchedulingStatusListViewModel[];


    @ViewChild(MatPaginator) paginator: MatPaginator;

    deadlineDropdown = [
        { name: 'Today', toDate: addDays(new Date(), 1).toString() },
        { name: 'Next week', toDate: addWeeks(new Date(), 1).toString() },
        { name: 'Next month', toDate: addMonths(new Date(), 1).toString() },
        { name: 'Next year', toDate: addYears(new Date(), 1).toString() }
    ];
     statusdeadlineDropdown = [
        { name: 'Today', toDate: addDays(new Date(), 1).toString() },
        { name: 'Next week', toDate: addWeeks(new Date(), 1).toString() },
        { name: 'Next month', toDate: addMonths(new Date(), 1).toString() },
        { name: 'Next year', toDate: addYears(new Date(), 1).toString() }
    ];

    statuses: IOrderStateListViewModel[];

    dataSource: PagedData<IOrdersListViewModel> = { items: [], pageIndex: 0, pageSize: 15 };

    constructor(
        public dialog: MatDialog,
        private formBuilder: FormBuilder,
        private store: Store<any>,
        private orderListService: OrderFinancesService,

    ) { }

    ngAfterViewInit(): void {
        this.paginator.page.pipe(
            tap((val) => this.store.dispatch(new ChangeFilter({ pager: { pageIndex: val.pageIndex, pageSize: val.pageSize } })))
        ).subscribe();
    }

    ngOnInit() {
        this.orderFiltersForm = this.formBuilder.group({
            currentStatus: '',
            orderId: '',
            workingNr: '',
            statusDeadline: undefined,
            deadline: undefined,
            customer: '',
            amount: undefined,
        });
        this.orderListService.getOrderFinanceStatuses().subscribe(res => this.statusesDropDown = res);

        this.orderFiltersForm.valueChanges.pipe(
            debounceTime(500),
            tap((values) => this.store.dispatch(new ChangeFilter(values))),
            takeUntil(this.destroy$)
        ).subscribe();

        this.store.pipe(
            select(coreLoginT),
            take(1),
            tap((resp) => {

            })
        ).subscribe();
        this.store.pipe(
            select(orderCustomerFilters),
            tap(val => this.orderFiltersForm.patchValue(val, { emitEvent: false })),
            tap(() => this.isLoading = true),
            switchMap((filter) =>
                this.store.pipe(
                    select(coreLoginT),
                    switchMap((token) =>
                        this.orderListService.getOrders(token.UserId, filter).pipe(
                            map(result => this.dataSource = result),
                            catchError(() => this.error = 'Error: could not load data'),
                            finalize(() => this.isLoading = false)
                        )
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

    clearFilter() {
        this.store.dispatch(new DeleteFilter());
    }



}
