import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { PagedData } from '../../../shared/models/paged-data.model';
import { ICargoListViewModel, ICargoStatusViewModel, ICargoSupplierCompanyViewModel, ICargoBookedByViewModel } from '../models/cargo.model';
import { MatPaginator } from '@angular/material/paginator';
import { CargoService } from '../services/cargo.service';
import { Store, select } from '@ngrx/store';
import { debounceTime, tap, takeUntil, switchMap, map, catchError, finalize } from 'rxjs/operators';
import { ChangeFilter, DeleteFilter } from '../store/actions/cargo.actions';
import { cargoFilters } from '../store/selectors/cargo.selector';
import { CargoStatusEnum, ClaimPolicyEnum } from '../../../shared/clients';
import { subDays, subWeeks, subMonths, subYears } from 'date-fns';
import { saveAs } from 'file-saver';

@Component({
    selector: 'factory-cargo-items',
    templateUrl: './cargo.page.component.html',
    styleUrls: ['./cargo.page.component.scss']
})
export class CargoPageComponent implements OnInit, OnDestroy, AfterViewInit {

    destroy$ = new Subject();

    stocked = CargoStatusEnum.Stocked;

    cargoForm: FormGroup;
    isLoading = false;
    error: string | null = null;
    claimPolicyEnum = ClaimPolicyEnum;


    arrivedOnDropdown = [
        { name: 'This day', toDate: subDays(new Date(), 1).toString() },
        { name: 'This week', toDate: subWeeks(new Date(), 1).toString() },
        { name: 'This month', toDate: subMonths(new Date(), 1).toString() },
        { name: 'This year', toDate: subYears(new Date(), 1).toString() }
    ];

    statusDropdown: ICargoStatusViewModel[];

    supplierDropdown: ICargoSupplierCompanyViewModel[];

    bookedByDropdown: ICargoBookedByViewModel[];

    @ViewChild(MatPaginator) paginator: MatPaginator;

    dataSource: PagedData<ICargoListViewModel> = { items: [], pageIndex: 0, pageSize: 15 };

    constructor(
        private formBuilder: FormBuilder,
        private store: Store<any>,
        private cargoService: CargoService
    ) { }

    ngAfterViewInit(): void {
        this.paginator.page.pipe(
            tap((val) => this.store.dispatch(new ChangeFilter({ pager: { pageIndex: val.pageIndex, pageSize: val.pageSize } })))
        ).subscribe();
    }

    ngOnInit() {
        this.cargoForm = this.formBuilder.group({
            cargoId: '',
            status: undefined,
            arrivedOn: undefined,
            supplier: undefined,
            bookedBy: undefined
        });
        this.cargoForm.valueChanges.pipe(
            debounceTime(500),
            tap((values) => {
                if (values.status === null) {
                    values.status = undefined;
                }
                this.store.dispatch(new ChangeFilter(values));
            }),
            takeUntil(this.destroy$)
        ).subscribe();

        this.store.pipe(
            select(cargoFilters),
            tap(val => this.cargoForm.patchValue(val, { emitEvent: false })),
            tap(() => this.isLoading = true),
            switchMap((filter) =>
                this.cargoService.getCargos(filter).pipe(
                    map(result => this.dataSource = result),
                    catchError(() => this.error = 'Error: could not load data'),
                    finalize(() => this.isLoading = false)
                ),
            ),
            switchMap((filter) =>
                this.cargoService.getCargoStatuses().pipe(
                    map(result => this.statusDropdown = result),
                    catchError(() => this.error = 'Error: could not load data'),
                    finalize(() => this.isLoading = false)
                ),
            ),
            switchMap((filter) =>
                this.cargoService.getCargoSupplierCompanies().pipe(
                    map(result => this.supplierDropdown = result),
                    catchError(() => this.error = 'Error: could not load data'),
                    finalize(() => this.isLoading = false)
                ),
            ),
            switchMap((filter) =>
                this.cargoService.getCargoBookedBy().pipe(
                    map(result => this.bookedByDropdown = result),
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

    downloadCsv() {
        this.cargoService.downloadCsv(this.cargoForm.value).subscribe(res => {
            saveAs(res.data, 'Cargos.csv');
        });
    }

}



