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
import { IGeneralExpensesListViewModel } from '../models/general-expenses.model';
import { Store, select } from '@ngrx/store';
import { GeneralExpensesService } from '../services/general-expenses.service';
import {
  tap,
  debounceTime,
  takeUntil,
  switchMap,
  map,
  catchError,
  finalize
} from 'rxjs/operators';
import {
  ChangeFilter,
  DeleteFilter
} from '../store/actions/general-expenses.actions';
import { generalExpensesFilters } from '../store/selectors/general-expenses.selector';
import { SnackbarService } from 'butor-shared-lib';
import { NewExpenseComponent } from '../components/new-expense/new-expense.component';
import { subDays, subWeeks, subMonths, subYears } from 'date-fns';
import { ClaimPolicyEnum } from '../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';

@Component({
  templateUrl: './general-expenses.page.component.html',
  styleUrls: ['./general-expenses.page.component.scss']
})
export class GeneralExpensesPageComponent
  implements OnInit, OnDestroy, AfterViewInit {
  destroy$ = new Subject();
  claimPolicyEnum = ClaimPolicyEnum;
  generalExpensesForm: FormGroup;
  isLoading = false;
  error: string | null = null;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  dataSource: PagedData<IGeneralExpensesListViewModel> = {
    items: [],
    pageIndex: 0,
    pageSize: 15
  };

  dateDropdown = [
    { name: 'This day', toDate: subDays(new Date(), 1).toString() },
    { name: 'This week', toDate: subWeeks(new Date(), 1).toString() },
    { name: 'This month', toDate: subMonths(new Date(), 1).toString() },
    { name: 'This year', toDate: subYears(new Date(), 1).toString() }
  ];

  constructor(
    private formBuilder: FormBuilder,
    private store: Store<any>,
    private generalExpensesService: GeneralExpensesService,
    public dialog: MatDialog,
    public snackBar: SnackbarService,
    private translate: TranslateService
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
    this.generalExpensesForm = this.formBuilder.group({
      name: '',
      amount: undefined,
      paymentDate: undefined
    });

    this.generalExpensesForm.valueChanges
      .pipe(
        debounceTime(500),
        tap(values => this.store.dispatch(new ChangeFilter(values))),
        takeUntil(this.destroy$)
      )
      .subscribe();

    this.store
      .pipe(
        select(generalExpensesFilters),
        tap(val =>
          this.generalExpensesForm.patchValue(val, { emitEvent: false })
        ),
        tap(() => (this.isLoading = true)),
        switchMap(filter =>
          this.generalExpensesService.getGeneralExpenses(filter).pipe(
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

  clearFilter() {
    this.store.dispatch(new DeleteFilter());
  }

  addNewExpense() {
    const dialogRef = this.dialog.open(NewExpenseComponent, {
      width: '48rem'
    });

    dialogRef.afterClosed().subscribe(res => {
      if (res !== undefined) {
        this.generalExpensesService.postGeneralExpense(res).subscribe(() => {
          this.snackBar.customMessage(this.translate.instant('snackbar.success'));
          this.clearFilter();
        });
      }
    });
  }

  editExpense(expenseId: number) {
    this.isLoading = true;
    this.generalExpensesService.getGeneralExpense(expenseId).subscribe(
      res => {
        const dialogRef = this.dialog.open(NewExpenseComponent, {
          width: '48rem',
          data: {
            id: expenseId,
            data: res
          }
        });

        dialogRef.afterClosed().subscribe(result => {
          if (result !== undefined) {
            this.generalExpensesService
              .putGeneralExpense(expenseId, result)
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
    this.generalExpensesService.deleteGeneralExpense(id).subscribe(res => {
      this.snackBar.customMessage(this.translate.instant('snackbar.deleted'));
      this.clearFilter();
    });
  }
}
