import {
  Component,
  OnInit,
  OnDestroy,
  AfterViewInit,
  ViewChild
} from '@angular/core';
import { ICellsListViewModel } from '../models/cells.model';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { TranslateService } from '@ngx-translate/core';
import { NewCellComponent } from '../components/new-cell/new-cell.component';
import { Subject } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import {
  tap,
  debounceTime,
  takeUntil,
  switchMap,
  map,
  catchError,
  finalize
} from 'rxjs/operators';
import { Store, select } from '@ngrx/store';
import { ChangeFilter, DeleteFilter } from '../store/actions/cells.actions';
import { CellsService } from '../services/cells.service';
import { cellFilters } from '../store/selectors/cell-list.selector';
import { SnackbarService } from 'butor-shared-lib';
import { ClaimPolicyEnum } from '../../../shared/clients';

@Component({
  selector: 'factory-cells',
  templateUrl: './cells.component.html',
  styleUrls: ['./cells.component.scss']
})
export class CellsComponent implements OnInit, OnDestroy, AfterViewInit {
  destroy$ = new Subject();

  cellsForm: FormGroup;
  isLoading = false;
  error: string | null = null;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  claimPolicyEnum = ClaimPolicyEnum;
  dataSource: PagedData<ICellsListViewModel> = {
    items: [],
    pageIndex: 0,
    pageSize: 15
  };

  constructor(
    private formBuilder: FormBuilder,
    public dialog: MatDialog,
    private store: Store<any>,
    private translate: TranslateService,
    private cellService: CellsService,
    public snackBar: SnackbarService
  ) {}

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
    this.cellsForm = this.formBuilder.group({
      name: '',
      stock: '',
      description: ''
    });

    this.cellsForm.valueChanges
      .pipe(
        debounceTime(500),
        tap(values => this.store.dispatch(new ChangeFilter(values))),
        takeUntil(this.destroy$)
      )
      .subscribe();

    this.store
      .pipe(
        select(cellFilters),
        tap(val => this.cellsForm.patchValue(val, { emitEvent: false })),
        tap(() => (this.isLoading = true)),
        switchMap(filter =>
          this.cellService.getCells(filter).pipe(
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

  addNewCell(): void {
    const dialogRef = this.dialog.open(NewCellComponent, {
      width: '48rem'
    });

    dialogRef.afterClosed().subscribe(res => {
      if (res !== undefined) {
        this.cellService.postCell(res).subscribe(() => {
          this.snackBar.customMessage(this.translate.instant('snackbar.success'));
          this.clearFilter();
        });
      }
    });
  }

  editCell(cellId: number) {
    this.isLoading = true;
    this.cellService.getCell(cellId).subscribe(
      res => {
        const dialogRef = this.dialog.open(NewCellComponent, {
          width: '48rem',
          data: {
            id: cellId,
            data: res
          }
        });

        dialogRef.afterClosed().subscribe(result => {
          if (result !== undefined) {
            this.cellService.putCell(cellId, result).subscribe(() => {
              this.snackBar.customMessage(this.translate.instant('snackbar.saved'));
              this.clearFilter();
            });
          }
        });
      },
      () => {},
      () => (this.isLoading = false)
    );
  }

  deleteButton(id: number) {
    this.cellService.deleteCell(id).subscribe(res => {
      this.snackBar.customMessage(this.translate.instant('snackbar.deleted'));
      this.clearFilter();
    });
  }

  clearFilter() {
    this.store.dispatch(new DeleteFilter());
  }
}
