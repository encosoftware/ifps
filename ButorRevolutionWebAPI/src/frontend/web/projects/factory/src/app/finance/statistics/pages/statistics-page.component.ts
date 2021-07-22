import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Store, select } from '@ngrx/store';
import { tap, takeUntil, switchMap, catchError, map, finalize, debounceTime } from 'rxjs/operators';
import { statisticsFilters } from '../store/selectors/statistics-list.selector';
import { StatisticsService } from '../services/statistics.service';
import { IStatisticsListModel } from '../models/statistics.model';
import { ChangeFilter } from '../store/actions/statistics-list.actions';

export interface IListMenu {
    year: number;
    name: number;
}

@Component({
    selector: 'factory-statistics-page',
    templateUrl: './statistics-page.component.html',
    styleUrls: ['./statistics-page.component.scss']
})
export class StatisticsPageComponent implements OnInit, OnDestroy, AfterViewInit {
    destroy$ = new Subject();

    statisticsForm: FormGroup;
    isLoading = false;
    error: string | null = null;

    dataSource: IStatisticsListModel[] = [];
    dates: IListMenu[] = [];
    pageCount = 0;
    constructor(
        private formBuilder: FormBuilder,
        private store: Store<any>,
        private statisticsService: StatisticsService
    ) {
        this.statisticsService.getOldestStatisticsYear().subscribe(year => {
            let currentYear = new Date().getFullYear();
            for (let i = currentYear; i >= year; i--) {
                let temp: IListMenu = {
                    name: i,
                    year: i
                };
                this.dates = [...this.dates, temp];
            }
        });
    }

    ngAfterViewInit(): void {

    }

    ngOnInit() {
        this.statisticsForm = this.formBuilder.group({
            year: new Date().getFullYear()
        });

        this.statisticsForm.valueChanges.pipe(
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
            select(statisticsFilters),
            tap(val => this.statisticsForm.patchValue(val, { emitEvent: false })),
            tap(() => this.isLoading = true),
            switchMap((filter) =>
                this.statisticsService.getFinanceStatistics(filter).pipe(
                    map(result => (this.dataSource = result.sort((x, y) => {
                        if (x.month < y.month) {
                            return -1;
                        }

                        if (x.month > y.month) {
                            return 1;
                        }

                        return 0;
                    }))),
                    catchError(() => this.error = 'Error: could not load data'),
                    finalize(() => this.isLoading = false),
                    tap((res) => {
                        this.pageCount = res.length % 12;
                    })
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
}
