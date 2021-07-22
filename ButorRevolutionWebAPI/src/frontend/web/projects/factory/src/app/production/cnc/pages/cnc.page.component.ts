import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { subDays, subWeeks, subMonths, subYears } from 'date-fns';
import { MatPaginator } from '@angular/material/paginator';
import { PagedData } from '../../../shared/models/paged-data.model';
import { ICncListViewModel } from '../models/cnc.model';
import { Store, select } from '@ngrx/store';
import { CncService } from '../services/cnc.service';
import { tap, debounceTime, takeUntil, switchMap, map, catchError, finalize } from 'rxjs/operators';
import { ChangeFilter, DeleteFilter } from '../store/actions/cncs.actions';
import { cncFilters } from '../store/selectors/cnc-list.selector';
import { ClaimPolicyEnum } from '../../../shared/clients';
import { ProductionService } from '../../../shared/services/production.service';

@Component({
    templateUrl: './cnc.page.component.html',
    styleUrls: ['./cnc.page.component.scss']
})
export class CncPageComponent implements OnInit, OnDestroy, AfterViewInit {
    destroy$ = new Subject();

    claimPolicyEnum = ClaimPolicyEnum;
    cncForm: FormGroup;
    isLoading = false;
    error: string | null = null;

    estimatedStartDropdown = [
        { name: 'This day', toDate: subDays(new Date(), 1).toString() },
        { name: 'This week', toDate: subWeeks(new Date(), 1).toString() },
        { name: 'This month', toDate: subMonths(new Date(), 1).toString() },
        { name: 'This year', toDate: subYears(new Date(), 1).toString() }
    ];

    @ViewChild(MatPaginator) paginator: MatPaginator;

    dataSource: PagedData<ICncListViewModel> = { items: [], pageIndex: 0, pageSize: 15 };

    constructor(
        private formBuilder: FormBuilder,
        private store: Store<any>,
        private cncService: CncService,
        private productionService: ProductionService
    ) { }

    ngAfterViewInit(): void {
        this.paginator.page.pipe(
            tap((val) => this.store.dispatch(new ChangeFilter({ pager: { pageIndex: val.pageIndex, pageSize: val.pageSize } })))
        ).subscribe();
    }

    ngOnInit() {
        this.loadData();
    }

    get hasError(): boolean {
        return !!this.error;
    }

    setProcessStatus(id: number) {
        this.productionService.setProcessStatus(id).subscribe(() => this.loadData());
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.unsubscribe();
    }

    clearFilter() {
        this.store.dispatch(new DeleteFilter());
    }

    loadData() {
        this.cncForm = this.formBuilder.group({
            componentId: '',
            material: '',
            orderId: '',
            workingNr: '',
            estimatedStartTime: undefined,
            workerName: ''
        });

        this.cncForm.valueChanges.pipe(
            debounceTime(500),
            tap((values) => this.store.dispatch(new ChangeFilter(values))),
            takeUntil(this.destroy$)
        ).subscribe();

        this.store.pipe(
            select(cncFilters),
            tap(val => this.cncForm.patchValue(val, { emitEvent: false })),
            tap(() => this.isLoading = true),
            switchMap((filter) =>
                this.cncService.getCncs(filter).pipe(
                    map(result => this.dataSource = result),
                    catchError(() => this.error = 'Error: could not load data'),
                    finalize(() => this.isLoading = false)
                ),
            ),
            takeUntil(this.destroy$)
        ).subscribe();
    }

}
