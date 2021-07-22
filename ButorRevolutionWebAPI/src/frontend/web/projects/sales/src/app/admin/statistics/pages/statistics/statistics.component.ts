import { Component, OnInit, OnDestroy } from '@angular/core';
import { StatisticsService } from '../../services/statistics.service';
import { SalesPersonStatisticsModel, IStatisticsFilterViewModel } from '../../models/statistics';
import { Store, select } from '@ngrx/store';
import { statisticsFilters } from '../../store/selectors/statistics.selector';
import { take, switchMap, map, tap, finalize, debounceTime, takeUntil } from 'rxjs/operators';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ChangeFilter } from '../../store/actions/statistics.actions';
import { Subject } from 'rxjs';
import { LanguageSetService } from '../../../../core/services/language-set.service';
import { DateAdapter } from '@angular/material/core';

@Component({
  selector: 'butor-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit, OnDestroy {

  salesPersonStatistics: SalesPersonStatisticsModel;
  isLoading: boolean;
  staticsFiltersForm: FormGroup;
  destroy$ = new Subject();

  constructor(
    private statisticsService: StatisticsService,
    private store: Store<any>,
    private formBuilder: FormBuilder,
    private lngService: LanguageSetService,
    private dateAdapter: DateAdapter<any>
  ) { }

  ngOnInit() {
    this.dateAdapter.setLocale(this.lngService.getLocalLanguageStorage());

    this.staticsFiltersForm = this.formBuilder.group({
      name: '',
      from: '',
      to: '',
    });

    this.staticsFiltersForm.valueChanges.pipe(
      debounceTime(500),
      tap(values => this.store.dispatch(new ChangeFilter(values))),
      takeUntil(this.destroy$)
    ).subscribe();

    this.store.pipe(
      select(statisticsFilters),
      tap(val => this.staticsFiltersForm.patchValue(val, { emitEvent: false })),
      tap(() => (this.isLoading = true)),
      switchMap((filter: IStatisticsFilterViewModel) =>
        this.statisticsService.getSalesPersonsList(filter).pipe(
          map(ins => this.salesPersonStatistics = ins),
          finalize(() => this.isLoading = false)
        )
      ),
      takeUntil(this.destroy$)
    ).subscribe();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }


}
