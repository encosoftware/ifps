import {
  Component,
  OnInit,
  OnDestroy,
  AfterViewInit,
  ViewChild
} from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { PagedData } from '../../../shared/models/paged-data.model';
import { DateAdapter } from '@angular/material/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
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
import { InspectionService } from '../services/inspection.service';
import { IInspectionListViewModel } from '../models/inspection.model';
import { TranslateService } from '@ngx-translate/core';
import {
  DeleteFilter,
  ChangeFilter
} from '../store/actions/inspection.actions';
import { inspectionFilters } from '../store/selectors/inspection.selector';
import { NewInspectionComponent } from '../components/new-inspection/new-inspection.component';
import { SnackbarService } from 'butor-shared-lib';
import { IStockDropDownViewModel } from '../../cells/models/cells.model';
import { ClaimPolicyEnum } from '../../../shared/clients';
import { LanguageSetService } from '../../../core/services/language-set.service';

@Component({
  selector: 'factory-inspection-items',
  templateUrl: './inspection.page.component.html',
  styleUrls: ['./inspection.page.component.scss']
})
export class InspectionPageComponent
  implements OnInit, OnDestroy, AfterViewInit {
  destroy$ = new Subject();

  inspectionForm: FormGroup;
  isLoading = false;
  error: string | null = null;

  lng: string;

  claimPolicyEnum = ClaimPolicyEnum;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  stocks: IStockDropDownViewModel[];

  dataSource: PagedData<IInspectionListViewModel> = {
    items: [],
    pageIndex: 0,
    pageSize: 15
  };

  constructor(
    private translate: TranslateService,
    private formBuilder: FormBuilder,
    private store: Store<any>,
    private inspectionService: InspectionService,
    public dialog: MatDialog,
    public snackBar: SnackbarService,
    private lngService: LanguageSetService,
    private dateAdapter: DateAdapter<any>
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
    this.lng = this.lngService.getLocalLanguageStorage();
    this.dateAdapter.setLocale(this.lng);
    this.inspectionForm = this.formBuilder.group({
      date: '',
      stock: '',
      report: '',
      delegation: ''
    });

    this.inspectionForm.valueChanges
      .pipe(
        debounceTime(500),
        tap(values => this.store.dispatch(new ChangeFilter(values))),
        takeUntil(this.destroy$)
      )
      .subscribe();

    this.store
      .pipe(
        select(inspectionFilters),
        tap(val => this.inspectionForm.patchValue(val, { emitEvent: false })),
        tap(() => (this.isLoading = true)),
        switchMap(filter =>
          this.inspectionService.getInspections(filter).pipe(
            map(result => (this.dataSource = result)),
            catchError(() => (this.error = 'Error: could not load data')),
            finalize(() => (this.isLoading = false))
          )
        ),
        switchMap(() =>
          this.inspectionService.getStocks().pipe(
            map(res => (this.stocks = res)),
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

  clearFilter() {
    this.store.dispatch(new DeleteFilter());
  }

  deleteButton(id: number) {
    this.inspectionService.deleteInspection(id).subscribe(() => {
      this.snackBar.customMessage(this.translate.instant('snackbar.deleted'));
      this.clearFilter();
    });
  }

  addNewInspection() {
    const dialogRef = this.dialog.open(NewInspectionComponent, {
      width: '48rem'
    });

    dialogRef.afterClosed().subscribe(res => {
      if (res !== undefined) {
        this.inspectionService.postInspection(res).subscribe(() => {
          this.snackBar.customMessage(this.translate.instant('snackbar.success'));
          this.clearFilter();
        });
      }
    });
  }

  editInspection(inspectionId: number) {
    this.isLoading = true;
    this.inspectionService.getInspection(inspectionId).subscribe(
      res => {
        const dialogRef = this.dialog.open(NewInspectionComponent, {
          width: '48rem',
          data: {
            id: inspectionId,
            data: res
          }
        });

        dialogRef.afterClosed().subscribe(result => {
          if (result !== undefined) {
            this.inspectionService
              .putInspection(inspectionId, result)
              .subscribe(() => {
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
}
