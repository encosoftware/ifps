import { Component, OnInit, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';
import { ICargoListViewModel } from '../models/cargo.model';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Router } from '@angular/router';
import { CargoService } from '../services/cargo.service';
import { Subscription, Subject } from 'rxjs';
import { debounceTime, tap, takeUntil, switchMap, finalize, map, catchError } from 'rxjs/operators';
import { Store, select } from '@ngrx/store';
import { ChangeFilter, DeleteFilter } from '../store/actions/cargo-list.actions';
import { cargoListFilters } from '../store/selectors/cargo-list.selector';
import { CargoStatusEnum, ClaimPolicyEnum } from '../../../shared/clients';
import { ISupplyDropModel } from '../../supply-orders/models/supply-orders.model';
import { SupplyService } from '../../supply-orders/services/supply.service';

@Component({
  selector: 'factory-cargo',
  templateUrl: './cargo.component.html',
  styleUrls: ['./cargo.component.scss']
})
export class CargoComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;

  dataSource: PagedData<ICargoListViewModel> = {
    items: [],
    pageIndex: 0,
    pageSize: 15
  };
  orderedStatusEnum = CargoStatusEnum.Ordered;
  cargoFiltersForm: FormGroup;
  cargoFiltersFormSub: Subscription;
  destroy$ = new Subject();
  isLoading = false;
  error: string | null = null;
  claimPolicyEnum = ClaimPolicyEnum;

  statusDropdown: ISupplyDropModel[];
  supplierDropdown: ISupplyDropModel[];

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private store: Store<any>,
    private service: CargoService,
    private supplyService: SupplyService
  ) { }

  ngAfterViewInit(): void {
    this.paginator.page
      .pipe(
        tap(val =>
          this.store.dispatch(
            new ChangeFilter({
              pager: { pageIndex: val.pageIndex, pageSize: val.pageSize }
            })
          )
        )
      )
      .subscribe();
  }

  ngOnInit() {
    this.loadData();
  }

  loadData(): void {
    this.isLoading = true;
    this.cargoFiltersForm = this.formBuilder.group({
      cargoId: '',
      status: null,
      created: '',
      supplier: null,
      bookedBy: null,
      totalCost: null
    });
    this.cargoFiltersFormSub = this.cargoFiltersForm.valueChanges
      .pipe(
        debounceTime(500),
        tap(values => this.store.dispatch(new ChangeFilter(values))),
        takeUntil(this.destroy$)
      )
      .subscribe();

    this.store
      .pipe(
        select(cargoListFilters),
        tap(val => this.cargoFiltersForm.patchValue(val, { emitEvent: false })),
        tap(() => (this.isLoading = true)),
        switchMap(filter =>
          this.service.listCargos(filter).pipe(
            map(res => (this.dataSource = res)),
            catchError(() => (this.error = 'Error: could not load data')),
            finalize(() => (this.isLoading = false))
          )
        ),
        switchMap(() =>
          this.service.getStatuses().pipe(
            map(res => (this.statusDropdown = res)),
            catchError(() => (this.error = 'Error: could not load data')),
            finalize(() => (this.isLoading = false))
          )
        ),
        switchMap(() =>
          this.supplyService.getCompanies().pipe(
            map(res => (this.supplierDropdown = res)),
            catchError(() => (this.error = 'Error: could not load data')),
            finalize(() => (this.isLoading = false))
          )
        ),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

  confrimationCargo(id: number): void {
    this.router.navigate(['/supply/cargo/confirmation/', id]);
  }

  arrivedCargo(id: number): void {
    const tempEnum =
      CargoStatusEnum[this.dataSource.items.find(x => x.id === id).statusEnum];
    if (CargoStatusEnum[tempEnum] !== CargoStatusEnum[this.orderedStatusEnum]) {
      this.router.navigate(['/supply/cargo/arrived/', id]);
    } else {
      this.router.navigate(['/supply/cargo/arrived/', id], { queryParams: { arrived: true } });
    }
  }

  clearFilter(): void {
    this.store.dispatch(new DeleteFilter());
  }

  deleteButton(id: number) {
    this.service.deleteCargo(id).subscribe(res => this.loadData());
  }

  get hasError(): boolean {
    return !!this.error;
  }
}
