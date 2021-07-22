import { Component, ViewChild, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { PagedData } from '../../../shared/models/paged-data.model';
import {
    IOrderSchedulingListViewModel,
    IOrderSchedulingStatusListViewModel,
    IProductionStatusDetailsViewModel
} from '../models/order-scheduling.model';
import { Store, select } from '@ngrx/store';
import { OrderSchedulingService } from '../services/order-scheduling.service';
import { tap, debounceTime, takeUntil, switchMap, map, catchError, finalize } from 'rxjs/operators';
import { ChangeFilter, DeleteFilter } from '../store/actions/order-schedulings.actions';
import { orderSchedulingFilters } from '../store/selectors/order-scheduling-list.selector';
import { addDays, addWeeks, addMonths, addYears } from 'date-fns';
import { OptimisationModalComponent } from '../components/optimisation-modal/optimisation-modal.component';
import { ClaimPolicyEnum, OrderStateEnum } from '../../../shared/clients';
import { Router } from '@angular/router';
import { PrintCodesComponent } from '../components/print-codes/print-codes.component';

@Component({
    templateUrl: './order-scheduling.page.component.html',
    styleUrls: ['./order-scheduling.page.component.scss']
})
export class OrderSchedulingPageComponent implements OnInit, OnDestroy, AfterViewInit {
    destroy$ = new Subject();

    orderSchedulingForm: FormGroup;
    isLoading = false;
    error: string | null = null;
    claimPolicyEnum = ClaimPolicyEnum;
    orderStateEnum = OrderStateEnum;
    allChecked = false;
    checkmarkClass = 'form__checkbox-button';

    statusesDropDown: IOrderSchedulingStatusListViewModel[];
    showPortal = false;
    prodDetails: IProductionStatusDetailsViewModel[] = [];
    deadlineDropdown = [
        { name: 'Today', toDate: addDays(new Date(), 1).toString() },
        { name: 'Next week', toDate: addWeeks(new Date(), 1).toString() },
        { name: 'Next month', toDate: addMonths(new Date(), 1).toString() },
        { name: 'Next year', toDate: addYears(new Date(), 1).toString() }
    ];

    @ViewChild(MatPaginator) paginator: MatPaginator;

    dataSource: PagedData<IOrderSchedulingListViewModel> = { items: [], pageIndex: 0, pageSize: 15 };

    constructor(
        public dialog: MatDialog,
        private formBuilder: FormBuilder,
        private store: Store<any>,
        private orderSchedulingService: OrderSchedulingService,
        private router: Router
    ) { }

    ngAfterViewInit(): void {
        this.paginator.page.pipe(
            tap((val) => this.store.dispatch(new ChangeFilter({ pager: { pageIndex: val.pageIndex, pageSize: val.pageSize } })))
        ).subscribe();
    }

    ngOnInit() {
        this.orderSchedulingForm = this.formBuilder.group({
            orderName: '',
            workingNr: '',
            statusId: undefined,
            deadline: undefined
        });
        this.orderSchedulingService.getOrderSchedulingStatuses().subscribe(res => this.statusesDropDown = res);

        this.orderSchedulingForm.valueChanges.pipe(
            debounceTime(500),
            tap((values) => this.store.dispatch(new ChangeFilter(values))),
            takeUntil(this.destroy$)
        ).subscribe();

        this.store.pipe(
            select(orderSchedulingFilters),
            tap(val => this.orderSchedulingForm.patchValue(val, { emitEvent: false })),
            tap(() => this.isLoading = true),
            switchMap((filter) =>
                this.orderSchedulingService.getOrderSchedulings(filter).pipe(
                    map(result => this.dataSource = result),
                    catchError(() => this.error = 'Error: could not load data'),
                    finalize(() => this.isLoading = false)
                ),
            ),
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

    toggleAllCheckbox(event) {
        this.checkmarkClass = 'form__checkbox-button';
        this.allChecked = event;
        for (let item of this.dataSource.items) {
            if (item.selectable === true) {
                item.isSelected = event;
            }
        }
    }

    toggleCheckbox() {
        let clickableItems = this.dataSource.items.filter(x => x.selectable === true);
        if (clickableItems.every(r => r.isSelected === true)) {
            this.checkmarkClass = 'form__checkbox-button';
        }
        if (clickableItems.every(r => r.isSelected === false)) {
            this.allChecked = false;
        }
        if (!clickableItems.every(r => r.isSelected === false) && !clickableItems.every(r => r.isSelected === true)) {
            this.checkmarkClass = 'form__checkbox-button-line';
            this.allChecked = true;
        }
    }

    isSelectedItem(): boolean {
        if (this.dataSource.items.findIndex(res => res.isSelected === true) > -1) {
            return false;
        } else {
            return true;
        }
    }

    findProdDetails(orderId: string): IProductionStatusDetailsViewModel {
        return this.prodDetails.find(x => x.orderId === orderId);
    }

    deleteFromProdDetails(deletedOrderId) {
        const index = this.prodDetails.indexOf(this.prodDetails.find(x => x.orderId === deletedOrderId));
        this.prodDetails.splice(index, 1);
    }

    optimisation() {
        const tempIds: string[] = [];
        this.dataSource.items.forEach(x => {
            if (x.isSelected) { tempIds.push(x.orderId); }
        });
        const dialogRef = this.dialog.open(OptimisationModalComponent, {
            width: '52rem',
            data: {
                orderIds: tempIds
            }
        });
        dialogRef.afterClosed().subscribe(res => {
            if (res !== undefined) {
                this.store.dispatch(new DeleteFilter());
            }
        });
    }

    freeUp(id: string) {
        this.orderSchedulingService.reserveOrFree(id, true).subscribe(res => {
            this.clearFilter();
        });
    }

    orderMaterials(id: string) {
        this.orderSchedulingService.orderMaterial(id).subscribe(res => {
            this.router.navigate(['/supply/orders']);
        });
    }

    reserve(id: string) {
        this.orderSchedulingService.reserveOrFree(id, false).subscribe(res => {
            this.clearFilter();
        });
    }

    printQrCode(id: number) {
        const dialogRef = this.dialog.open(PrintCodesComponent, {
            width: '93rem',
            data: {
                orderId: id
            }
        });

        dialogRef.afterClosed().subscribe();
    }
}
