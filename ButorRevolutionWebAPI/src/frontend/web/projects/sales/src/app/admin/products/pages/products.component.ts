import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Store, select } from '@ngrx/store';
import { tap, debounceTime, takeUntil, switchMap, map, catchError, finalize } from 'rxjs/operators';
import { IProductsListViewModel, IFurnitureUnitCategoryModel } from '../models/products.models';
import { ProductsService } from '../services/products.service';
import { productFilters } from '../store/selectors/products-list.selector';
import { ChangeFilter, DeleteFilter } from '../store/actions/products-list.actions';
import { NewProductUnitComponent } from '../components/new-product-unit/new-product-unit.component';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ClaimPolicyEnum } from '../../../shared/clients';

@Component({
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit, OnDestroy, AfterViewInit {
  destroy$ = new Subject();

  productsFiltersForm: FormGroup;
  isLoading = false;
  error: string | null = null;

  productCategories: IFurnitureUnitCategoryModel[];

  @ViewChild(MatPaginator) paginator: MatPaginator;

  dataSource: PagedData<IProductsListViewModel> = {
    items: [],
    pageIndex: 0,
    pageSize: 15
  };
  claimPolicyEnum = ClaimPolicyEnum;

  constructor(
    private router: Router,
    public dialog: MatDialog,
    private formBuilder: FormBuilder,
    private store: Store<any>,
    private translate: TranslateService,
    private productListService: ProductsService,
    private snackBar: MatSnackBar
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
    this.productsFiltersForm = this.formBuilder.group({
      description: '',
      code: '',
      category: '',
      categoryId: null
    });

    this.productsFiltersForm.valueChanges
      .pipe(
        debounceTime(500),
        tap(values => this.store.dispatch(new ChangeFilter(values))),
        takeUntil(this.destroy$)
      )
      .subscribe();

    this.store
      .pipe(
        select(productFilters),
        tap(val =>
          this.productsFiltersForm.patchValue(val, { emitEvent: false })
        ),
        tap(() => (this.isLoading = true)),
        switchMap(filter =>
          this.productListService.getProductsList(filter).pipe(
            map(result => (this.dataSource = result)),
            catchError(() => (this.error = 'Error: could not load data')),
            finalize(() => (this.isLoading = false))
          )
        ),
        takeUntil(this.destroy$)
      )
      .subscribe();
    this.productListService
      .getFurnitureUnitCategories()
      .subscribe(res => (this.productCategories = res));
  }

  get hasError(): boolean {
    return !!this.error;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

  addNewFurnitureUnit(): void {
    const dialogRef = this.dialog.open(NewProductUnitComponent, {
      width: '93rem'
    });

    dialogRef.afterClosed();
  }

  clearFilter() {
    this.store.dispatch(new DeleteFilter());
  }

  deleteProduct(id: string) {
    this.productListService
      .deleteProductUnit(id)
      .subscribe(res => this.loadData());
  }

}
