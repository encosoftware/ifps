import {
  Component,
  OnInit,
  OnDestroy,
  AfterViewInit,
  ViewChild
} from '@angular/core';
import { IStoragesListViewModel } from '../models/stocks.model';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { NewStorageComponent } from '../components/new-stock/new-stock.component';
import { Subject } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Store, select } from '@ngrx/store';
import { StoragesService } from '../services/stocks.service';
import {
  debounceTime,
  tap,
  takeUntil,
  switchMap,
  map,
  catchError,
  finalize
} from 'rxjs/operators';
import { ChangeFilter, DeleteFilter } from '../store/actions/stocks.actions';
import { storageFilters } from '../store/selectors/stock-list.selector';
import { SnackbarService } from 'butor-shared-lib';
import { ClaimPolicyEnum } from '../../../shared/clients';

@Component({
  templateUrl: './stocks.component.html',
  styleUrls: ['./stocks.component.scss']
})
export class StoragesComponent implements OnInit, OnDestroy, AfterViewInit {
  destroy$ = new Subject();

  storagesForm: FormGroup;
  isLoading = false;
  error: string | null = null;

  claimPolicyEnum = ClaimPolicyEnum;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  dataSource: PagedData<IStoragesListViewModel> = {
    items: [],
    pageIndex: 0,
    pageSize: 15
  };

  constructor(
    private formBuilder: FormBuilder,
    public dialog: MatDialog,
    private store: Store<any>,
    private stockService: StoragesService,
    public snackBar: SnackbarService
  ) { }

  ngOnInit() {
    this.storagesForm = this.formBuilder.group({
      company: '',
      name: '',
      address: ''
    });

    this.storagesForm.valueChanges
      .pipe(
        debounceTime(500),
        tap(values => this.store.dispatch(new ChangeFilter(values))),
        takeUntil(this.destroy$)
      )
      .subscribe();

    this.store
      .pipe(
        select(storageFilters),
        tap(val => this.storagesForm.patchValue(val, { emitEvent: false })),
        tap(() => (this.isLoading = true)),
        switchMap(filter =>
          this.stockService.getStorages(filter).pipe(
            map(result => (this.dataSource = result)),
            catchError(() => (this.error = 'Error: could not load data')),
            finalize(() => (this.isLoading = false))
          )
        ),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }

  get hasError(): boolean {
    return !!this.error;
  }

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

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

  addNewStorage(): void {
    const dialogRef = this.dialog.open(NewStorageComponent, {
      width: '48rem'
    });

    dialogRef.afterClosed().subscribe(res => {
      if (res !== undefined) {
        this.stockService.postStock(res).subscribe(() => {
          this.snackBar.customMessage('Stock added!');
          this.clearFilter();
        });
      }
    });
  }

  editStorage(stockId: number): void {
    this.isLoading = true;
    this.stockService.getStorage(stockId).subscribe(
      res => {
        const dialogRef = this.dialog.open(NewStorageComponent, {
          width: '48rem',
          data: {
            id: stockId,
            data: res
          }
        });

        dialogRef.afterClosed().subscribe(result => {
          if (result !== undefined) {
            this.stockService.putStock(stockId, result).subscribe(() => {
              this.snackBar.customMessage('Stock edited!');
              this.clearFilter();
            });
          }
        });
      },
      () => { },
      () => (this.isLoading = false)
    );
  }

  deleteButton(id: number) {
    this.stockService.deleteStock(id).subscribe(() => {
      this.snackBar.customMessage('Storage deleted!');
      this.clearFilter();
    });
  }

  clearFilter(): void {
    this.store.dispatch(new DeleteFilter());
  }
}
