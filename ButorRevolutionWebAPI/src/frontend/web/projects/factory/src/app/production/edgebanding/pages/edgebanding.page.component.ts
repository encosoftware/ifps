import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { subDays, subWeeks, subMonths, subYears } from 'date-fns';
import { MatPaginator } from '@angular/material/paginator';
import { PagedData } from '../../../shared/models/paged-data.model';
import { IEdgebandingListViewModel } from '../models/edgebanding.model';
import { Store, select } from '@ngrx/store';
import { EdgebandingService } from '../services/edgebanding.service';
import { tap, debounceTime, takeUntil, switchMap, map, catchError, finalize } from 'rxjs/operators';
import { ChangeFilter, DeleteFilter } from '../store/actions/edgebandings.actions';
import { edgebandingFilters } from '../store/selectors/edgebanding-list.selector';
import { ProductionService } from '../../../shared/services/production.service';

@Component({
    templateUrl: './edgebanding.page.component.html',
    styleUrls: ['./edgebanding.page.component.scss']
})
export class EdgebandingPageComponent implements OnInit, OnDestroy, AfterViewInit {
    destroy$ = new Subject();

    edgebandingForm: FormGroup;
    isLoading = false;
    error: string | null = null;

    estimatedStartDropdown = [
        { name: 'This day', toDate: subDays(new Date(), 1).toString() },
        { name: 'This week', toDate: subWeeks(new Date(), 1).toString() },
        { name: 'This month', toDate: subMonths(new Date(), 1).toString() },
        { name: 'This year', toDate: subYears(new Date(), 1).toString() }
    ];

    @ViewChild(MatPaginator) paginator: MatPaginator;

    dataSource: PagedData<IEdgebandingListViewModel> = { items: [], pageIndex: 0, pageSize: 15 };

    constructor(
        private formBuilder: FormBuilder,
        private store: Store<any>,
        private edgebandingService: EdgebandingService,
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

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.unsubscribe();
    }

    setProcessStatus(id: number) {
        this.productionService.setProcessStatus(id).subscribe(() => this.loadData());
    }

    clearFilter() {
        this.store.dispatch(new DeleteFilter());
    }

    loadData() {
        this.edgebandingForm = this.formBuilder.group({
            componentId: '',
            orderId: '',
            workingNr: '',
            estimatedStartTime: undefined,
            workerName: ''
        });

        this.edgebandingForm.valueChanges.pipe(
            debounceTime(500),
            tap((values) => this.store.dispatch(new ChangeFilter(values))),
            takeUntil(this.destroy$)
        ).subscribe();

        this.store.pipe(
            select(edgebandingFilters),
            tap(val => this.edgebandingForm.patchValue(val, { emitEvent: false })),
            tap(() => this.isLoading = true),
            switchMap((filter) =>
                this.edgebandingService.getEdgebandings(filter).pipe(
                    map(result => this.dataSource = result),
                    catchError(() => this.error = 'Error: could not load data'),
                    finalize(() => this.isLoading = false)
                ),
            ),
            takeUntil(this.destroy$)
        ).subscribe();
    }
}
