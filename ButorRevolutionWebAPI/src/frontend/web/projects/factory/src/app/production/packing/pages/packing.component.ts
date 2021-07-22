import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { subDays, subWeeks, subMonths, subYears } from 'date-fns';
import { MatPaginator } from '@angular/material/paginator';
import { PagedData } from '../../../shared/models/paged-data.model';
import { IPackingListViewModel } from '../models/packing.model';
import { Store, select } from '@ngrx/store';
import { tap, debounceTime, takeUntil, switchMap, map, catchError, finalize } from 'rxjs/operators';
import { packingFilters } from '../store/selectors/packing-list.selector';
import { ChangeFilter, DeleteFilter } from '../store/actions/packing.actions';
import { PackingService } from '../services/packing.service';
import { ProductionService } from '../../../shared/services/production.service';

@Component({
  selector: 'factory-packing',
  templateUrl: './packing.component.html',
  styleUrls: ['./packing.component.scss']
})
export class PackingPageComponent implements OnDestroy, AfterViewInit, OnInit {

  destroy$ = new Subject();

  packingForm: FormGroup;
  isLoading = false;
  error: string | null = null;

  estimatedStartDropdown = [
    { name: 'This day', toDate: subDays(new Date(), 1).toString() },
    { name: 'This week', toDate: subWeeks(new Date(), 1).toString() },
    { name: 'This month', toDate: subMonths(new Date(), 1).toString() },
    { name: 'This year', toDate: subYears(new Date(), 1).toString() }
  ];

  @ViewChild(MatPaginator) paginator: MatPaginator;

  dataSource: PagedData<IPackingListViewModel> = { items: [], pageIndex: 0, pageSize: 15 };

  constructor(
    private formBuilder: FormBuilder,
    private store: Store<any>,
    private packingService: PackingService,
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
    this.packingForm = this.formBuilder.group({
      unitId: '',
      orderId: '',
      workingNr: '',
      date: undefined,
      workerName: ''
    });

    this.packingForm.valueChanges.pipe(
      debounceTime(500),
      tap((values) => this.store.dispatch(new ChangeFilter(values))),
      takeUntil(this.destroy$)
    ).subscribe();

    this.store.pipe(
      select(packingFilters),
      tap(val => this.packingForm.patchValue(val, { emitEvent: false })),
      tap(() => this.isLoading = true),
      switchMap((filter) =>
        this.packingService.getPackingList(filter).pipe(
          map(result => this.dataSource = result),
          catchError(() => this.error = 'Error: could not load data'),
          finalize(() => this.isLoading = false)
        ),
      ),
      takeUntil(this.destroy$)
    ).subscribe();
  }

}
