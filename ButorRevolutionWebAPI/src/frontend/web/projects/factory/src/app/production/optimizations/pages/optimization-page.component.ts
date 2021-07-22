import { Component, ViewChild, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Store, select } from '@ngrx/store';
import { tap, debounceTime, takeUntil, switchMap, map, catchError, finalize } from 'rxjs/operators';
import { addDays, addWeeks, addMonths, addYears } from 'date-fns';
import { IOptimizationListViewModel } from '../models/optimization.model';
import { OptimizationService } from '../services/optimization.service';
import { ChangeFilter, DeleteFilter } from '../store/actions/optimizations.actions';
import { optimizationFilters } from '../store/selectors/optimization-list.selector';
import { saveAs } from 'file-saver';

@Component({
    templateUrl: './optimization-page.component.html',
    styleUrls: ['./optimization-page.component.scss']
})
export class OptimizationPageComponent implements OnInit, OnDestroy, AfterViewInit {
    destroy$ = new Subject();

    optimizationForm: FormGroup;
    isLoading = false;
    error: string | null = null;

    allChecked = false;
    checkmarkClass = 'form__checkbox-button';

    showPortal = false;
    startedAtDropdown = [
        { name: 'Today', toDate: addDays(new Date(), 1).toString() },
        { name: 'Next week', toDate: addWeeks(new Date(), 1).toString() },
        { name: 'Next month', toDate: addMonths(new Date(), 1).toString() },
        { name: 'Next year', toDate: addYears(new Date(), 1).toString() }
    ];

    @ViewChild(MatPaginator) paginator: MatPaginator;

    dataSource: PagedData<IOptimizationListViewModel> = { items: [], pageIndex: 0, pageSize: 15 };

    constructor(
        private formBuilder: FormBuilder,
        private store: Store<any>,
        private optimizationService: OptimizationService
    ) { }

    ngAfterViewInit(): void {
        this.paginator.page.pipe(
            tap((val) => this.store.dispatch(new ChangeFilter({ pager: { pageIndex: val.pageIndex, pageSize: val.pageSize } })))
        ).subscribe();
    }

    ngOnInit() {
        this.optimizationForm = this.formBuilder.group({
            id: '',
            shiftNumber: '',
            shiftLength: undefined,
            startedAt: undefined,
            deadline: undefined
        });
        this.optimizationForm.valueChanges.pipe(
            debounceTime(500),
            tap((values) => this.store.dispatch(new ChangeFilter(values))),
            takeUntil(this.destroy$)
        ).subscribe();

        this.store.pipe(
            select(optimizationFilters),
            tap(val => this.optimizationForm.patchValue(val, { emitEvent: false })),
            tap(() => this.isLoading = true),
            switchMap((filter) =>
                this.optimizationService.getOptimizations(filter).pipe(
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

    getLayoutZipFile(id: string) {
        this.optimizationService.getLayoutZipFile(id).subscribe(res => {
            saveAs(res.data, id);
        });
    }

    getScheduleZipFile(id: string) {
        this.optimizationService.getScheduleZipFile(id).subscribe(res => {
            saveAs(res.data, id);
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.unsubscribe();
    }

    clearFilter() {
        this.store.dispatch(new DeleteFilter());
    }
}
