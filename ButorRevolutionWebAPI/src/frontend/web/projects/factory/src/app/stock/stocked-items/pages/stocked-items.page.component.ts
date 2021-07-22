import {
  Component,
  OnInit,
  OnDestroy,
  AfterViewInit,
  ViewChild
} from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { PagedData } from '../../../shared/models/paged-data.model';
import { IStockedItemListViewModel } from '../models/stocked-items.model';
import { Store, select } from '@ngrx/store';
import {
  debounceTime,
  tap,
  takeUntil,
  switchMap,
  map,
  catchError,
  finalize
} from 'rxjs/operators';
import { StockedItemsService } from '../services/stocked-items.service';
import {
  ChangeFilter,
  DeleteFilter
} from '../store/actions/stocked-items.actions';
import { stockedItemFilters } from '../store/selectors/stocked-items.selector';
import { NewStockedItemComponent } from '../components/new-stocked-item/new-stocked-item.component';
import { Router } from '@angular/router';
import { SnackbarService } from 'butor-shared-lib';
import { ClaimPolicyEnum } from '../../../shared/clients';
import { saveAs } from 'file-saver';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'factory-stocked-items',
  templateUrl: './stocked-items.page.component.html',
  styleUrls: ['./stocked-items.page.component.scss']
})
export class StockedItemsPageComponent
  implements OnInit, OnDestroy, AfterViewInit {
  destroy$ = new Subject();

  stockedItemsForm: FormGroup;
  isLoading = false;
  error: string | null = null;

  allChecked = false;
  checkmarkClass = 'form__checkbox-button';

  @ViewChild(MatPaginator) paginator: MatPaginator;

  claimPolicyEnum = ClaimPolicyEnum;
  dataSource: PagedData<IStockedItemListViewModel> = {
    items: [],
    pageIndex: 0,
    pageSize: 15
  };

  constructor(
    private formBuilder: FormBuilder,
    public dialog: MatDialog,
    private store: Store<any>,
    private translate: TranslateService,
    private router: Router,
    private stockedService: StockedItemsService,
    public snackBar: SnackbarService
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
    this.stockedItemsForm = this.formBuilder.group({
      checkbox: '',
      description: '',
      code: '',
      cellName: '',
      cellMeta: '',
      quantity: undefined
    });

    this.stockedItemsForm.valueChanges
      .pipe(
        debounceTime(500),
        tap(values => {
          if (values.quantity === '') {
            values.quantity = undefined;
          }
          this.store.dispatch(new ChangeFilter(values));
        }),
        takeUntil(this.destroy$)
      )
      .subscribe();

    this.store
      .pipe(
        select(stockedItemFilters),
        tap(val => this.stockedItemsForm.patchValue(val, { emitEvent: false })),
        tap(() => (this.isLoading = true)),
        switchMap(filter =>
          this.stockedService.getStockedItems(filter).pipe(
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

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

  downloadCsv() {
    this.stockedService.downloadCsv(this.stockedItemsForm.value).subscribe(res => {
      saveAs(res.data, 'StockedItems.csv');
    });
  }

  isDisable(): boolean {
    if (!this.dataSource.items) {
      return true;
    }
    if (this.dataSource.items.findIndex(res => res.isSelected === true) > -1) {
      return false;
    } else {
      return true;
    }
  }

  eject() {
    this.stockedService.updatedDataSelection(
      this.dataSource.items.filter(res => res.isSelected === true)
    );
    this.router.navigate(['/stock/stockeditems/eject']);
  }

  reserve() {
    this.stockedService.updatedDataSelection(
      this.dataSource.items.filter(res => res.isSelected === true)
    );
    this.router.navigate(['/stock/stockeditems/reserve']);
  }

  addNewStockedItem(): void {
    const dialogRef = this.dialog.open(NewStockedItemComponent, {
      width: '48rem'
    });

    dialogRef.afterClosed().subscribe(res => {
      if (res !== undefined) {
        this.stockedService.postStockedItem(res).subscribe(() => {
          this.snackBar.customMessage(this.translate.instant('snackbar.success'));
          this.clearFilter();
        });
      }
    });
  }

  editStockedItem(stockedItemId: number): void {
    this.isLoading = true;
    this.stockedService.getStockedItem(stockedItemId).subscribe(
      res => {
        const dialogRef = this.dialog.open(NewStockedItemComponent, {
          width: '48rem',
          data: {
            id: stockedItemId,
            data: res
          }
        });

        dialogRef.afterClosed().subscribe(result => {
          if (result !== undefined) {
            this.stockedService
              .putStockedItem(stockedItemId, result)
              .subscribe(() => {
                this.snackBar.customMessage(this.translate.instant('snackbar.saved'));
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
    this.stockedService.deleteStockedItem(id).subscribe(() => {
      this.snackBar.customMessage(this.translate.instant('snackbar.deleted'));
      this.clearFilter();
    });
  }

  clearFilter() {
    this.store.dispatch(new DeleteFilter());
  }

  toggleAllCheckbox(event) {
    this.checkmarkClass = 'form__checkbox-button';
    this.allChecked = event;
    for (let item of this.dataSource.items) {
      item.isSelected = event;
    }
  }

  toggleCheckbox() {
    if (this.dataSource.items.every(r => r.isSelected === true)) {
      this.checkmarkClass = 'form__checkbox-button';
    }
    if (this.dataSource.items.every(r => r.isSelected === false)) {
      this.allChecked = false;
    }
    if (
      !this.dataSource.items.every(r => r.isSelected === false) &&
      !this.dataSource.items.every(r => r.isSelected === true)
    ) {
      this.checkmarkClass = 'form__checkbox-button-line';
      this.allChecked = true;
    }
  }
}
