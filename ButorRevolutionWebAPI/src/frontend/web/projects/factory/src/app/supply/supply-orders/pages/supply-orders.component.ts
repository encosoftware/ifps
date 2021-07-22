import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { ISupplyOrderListViewModel, ISupplyDropdownModel, ISupplyDropModel } from '../models/supply-orders.model';
import { MatPaginator } from '@angular/material/paginator';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { SupplyService } from '../services/supply.service';
import { Subscription, Subject } from 'rxjs';
import { debounceTime, tap, takeUntil, switchMap, map, catchError, finalize } from 'rxjs/operators';
import { Store, select } from '@ngrx/store';
import { supplyOrdersFilters } from '../store/selectors/supply-order-list.selector';
import { DeleteFilter, ChangeFilter } from '../store/actions/supply-order-list.actions';
import { PagedData } from '../../../shared/models/paged-data.model';
import { DataService } from '../../../shared/services/data.service';
import { ClaimPolicyEnum } from '../../../shared/clients';
import { saveAs } from 'file-saver';

@Component({
  selector: 'factory-supply-orders',
  templateUrl: './supply-orders.component.html',
  styleUrls: ['./supply-orders.component.scss']
})
export class SupplyOrdersComponent implements OnInit, OnDestroy, AfterViewInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;

  dataSource: PagedData<ISupplyOrderListViewModel> = { items: [], pageIndex: 0, pageSize: 15 };

  supplyFiltersForm: FormGroup;
  supplyFiltersFormSub: Subscription;
  destroy$ = new Subject();
  isLoading = false;
  allIsChecked = false;
  error: string | null = null;
  claimPolicyEnum = ClaimPolicyEnum;
  checkedOrders: ISupplyOrderListViewModel[] = [];
  actualSupplier: number;
  actualSupplierSelect: boolean;
  materialsDropdown: ISupplyDropdownModel[];
  supplierDropdown: ISupplyDropModel[];

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private store: Store<any>,
    private service: SupplyService,
    private dataService: DataService,
  ) { }

  ngAfterViewInit(): void {
    this.paginator.page.pipe(
      tap((val) => this.store.dispatch(new ChangeFilter({ pager: { pageIndex: val.pageIndex, pageSize: val.pageSize } })))
    ).subscribe();
  }

  ngOnInit() {
    this.supplyFiltersForm = this.formBuilder.group({
      orderId: '',
      workingNumber: '',
      material: '',
      name: '',
      supplier: null,
      deadline: null
    });

    this.supplyFiltersFormSub = this.supplyFiltersForm.valueChanges.pipe(
      debounceTime(500),
      tap((values) => this.store.dispatch(new ChangeFilter(values))),
      takeUntil(this.destroy$)
    ).subscribe();

    this.store.pipe(
      select(supplyOrdersFilters),
      tap(val => this.supplyFiltersForm.patchValue(val, { emitEvent: false })),
      tap(() => this.isLoading = true),
      switchMap((filter) =>
        this.service.listOrders(filter).pipe(
          map(res => this.dataSource = res),
          catchError(() => this.error = 'Error: could not load data'),
          finalize(() => this.isLoading = false)
        )
      ),
      switchMap(() =>
        this.service.getMaterials().pipe(
          map(res => {
            this.materialsDropdown = [];
            this.materialsDropdown = res;
          }),
          catchError(() => this.error = 'Error: could not load data'),
          finalize(() => this.isLoading = false)
        )
      ),
      switchMap(() =>
        this.service.getCompanies().pipe(
          map(res => {
            this.supplierDropdown = [];
            this.supplierDropdown = res;
          }),
          catchError(() => this.error = 'Error: could not load data'),
          finalize(() => this.isLoading = false)
        )
      ),
      takeUntil(this.destroy$),
    ).subscribe();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

  createCargo(): void {
    const cargoIds: number[] = [];
    this.checkedOrders.forEach(x => {
      cargoIds.push(x.id);
    });
    this.dataService.changeIds(cargoIds);
    if (cargoIds.length !== 0) {
      this.router.navigate(['/supply/orders/create-cargo']);
    }
  }

  toggleOrder(id: number, event: Event) {
    this.checkedOrders = [];
    this.actualSupplierSelect = !!this.dataSource.items.find(x => x.id === id).supplier.find(y => y.id === this.actualSupplier);
    if (this.actualSupplierSelect) {
      this.dataSource.items.find(x => x.id === id).isChecked = !this.dataSource.items.find(x => x.id === id).isChecked;
      this.dataSource.items.forEach(x => {
        if (x.isChecked) {
          this.checkedOrders.push(x);
        }
      });
    }
  }

  downloadCsv() {
    this.service.downloadCsv(this.supplyFiltersForm.value ).subscribe(res => {
      saveAs(res.data, 'RequiredMaterials.csv');
    });
  }

  toggleAll(event: Event) {
    this.checkedOrders = [];
    event.stopPropagation();
    this.allIsChecked = !this.allIsChecked;
    if (this.actualSupplier) {
      this.dataSource.items.forEach((x) => {
        x.supplier.forEach(y => {
          if (y.id === this.actualSupplier) {
            x.isChecked = this.allIsChecked;
            if (x.isChecked) {
              if (!this.checkedOrders.includes(x)) {
                this.checkedOrders.push(x);
              }
            }
          }
        })
      });

    }
  }

  clearFilter(): void {
    this.store.dispatch(new DeleteFilter());
  }

  get hasError(): boolean {
    return !!this.error;
  }

  cargoChange() {
    this.checkedOrders = [];
    this.dataSource.items.forEach(element => {
      element.isChecked = false;
    });
    this.dataService.supplyerID = this.actualSupplier;
  }
}
