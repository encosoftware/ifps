import { Component, OnInit, OnDestroy, Optional, Inject } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { tap, takeUntil, debounceTime, switchMap, map } from 'rxjs/operators';
import { Subject, forkJoin } from 'rxjs';
import { trendsFilters } from '../../store/selectors/trend-list.selector';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ChangeFilter } from '../../store/actions/trend-list.actions';
import { TrendsService } from '../../services/trends.service';
import { ICorpusTrendsListViewModel, IFrontTrendsListViewModel, IFurnitureUnitTrendsListViewModel } from '../../models/trends.models';
import { API_BASE_URL } from '../../../../shared/clients';
import { LanguageSetService } from '../../../../core/services/language-set.service';
import { DateAdapter } from '@angular/material/core';


@Component({
  selector: 'butor-trends',
  templateUrl: './trends.component.html',
  styleUrls: ['./trends.component.scss']
})
export class TrendsComponent implements OnInit, OnDestroy {

  destroy$ = new Subject();
  trendsForm: FormGroup;

  corpuses: ICorpusTrendsListViewModel[] = [];
  fronts: IFrontTrendsListViewModel[] = [];
  furnitureUnits: IFurnitureUnitTrendsListViewModel[] = [];

  constructor(
    private store: Store<any>,
    private formBuilder: FormBuilder,
    private service: TrendsService,
    private lngService: LanguageSetService,
    private dateAdapter: DateAdapter<any>,
    @Optional() @Inject(API_BASE_URL) public baseUrl?: string
  ) { }

  ngOnInit() {
    this.dateAdapter.setLocale(this.lngService.getLocalLanguageStorage());
    this.trendsForm = this.formBuilder.group({
      intervalFrom: new Date(2019, 11, 31),
      intervalTo: new Date(2020, 11, 31),
      take: 10
    });

    this.trendsForm.valueChanges.pipe(
      tap(values => this.store.dispatch(new ChangeFilter(values))),
      takeUntil(this.destroy$)
    ).subscribe();

    this.store.pipe(
      select(trendsFilters),
      switchMap(filter =>
        forkJoin(
          this.service.getCorpusTrends(filter),
          this.service.getFrontTrends(filter),
          this.service.getFurnitureUnitTrends(filter)
        )
      ),
      takeUntil(this.destroy$)
    ).subscribe(([corpus, front, fu]) => {
      this.corpuses = corpus;
      this.fronts = front;
      this.furnitureUnits = fu;
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

}
