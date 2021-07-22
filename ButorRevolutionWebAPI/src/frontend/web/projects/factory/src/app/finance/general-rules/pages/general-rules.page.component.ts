import {
  Component,
  ViewChild,
  OnInit,
  OnDestroy,
  AfterViewInit
} from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { IGeneralRulesListViewModel } from '../models/general-rules.model';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Store, select } from '@ngrx/store';
import {
  tap,
  debounceTime,
  takeUntil,
  switchMap,
  map,
  catchError
} from 'rxjs/operators';
import {
  ChangeFilter,
  DeleteFilter
} from '../store/actions/general-rules.actions';
import { generalRulesFilters } from '../store/selectors/general-rules.selector';
import { GeneralRulesService } from '../services/general-rules.service';
import { SnackbarService } from 'butor-shared-lib';
import { NewRuleComponent } from '../components/new-rule/new-rule.component';
import { IFrequencyListViewModel } from '../../general-expenses/models/general-expenses.model';
import { subDays, subWeeks, subMonths, subYears } from 'date-fns';
import { ClaimPolicyEnum } from '../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';

@Component({
  templateUrl: './general-rules.page.component.html',
  styleUrls: ['./general-rules.page.component.scss']
})
export class GeneralRulesPageComponent
  implements OnInit, OnDestroy, AfterViewInit {
  destroy$ = new Subject();
  claimPolicyEnum = ClaimPolicyEnum;
  generalRulesForm: FormGroup;
  isLoading = false;
  error: string | null = null;

  frequencyDropdown: IFrequencyListViewModel[];

  @ViewChild(MatPaginator) paginator: MatPaginator;

  dataSource: PagedData<IGeneralRulesListViewModel> = {
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
    private generalRulesService: GeneralRulesService,
    public snackBar: SnackbarService,
    public translate: TranslateService,
    public dialog: MatDialog
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
    this.generalRulesForm = this.formBuilder.group({
      name: '',
      amount: undefined,
      frequencyFrom: undefined,
      frequencyTo: undefined,
      frequencyTypeId: undefined,
      startDate: undefined
    });
    this.generalRulesForm.valueChanges
      .pipe(
        debounceTime(500),
        tap(values => this.store.dispatch(new ChangeFilter(values))),
        takeUntil(this.destroy$)
      )
      .subscribe();

    this.store
      .pipe(
        select(generalRulesFilters),
        tap(val => {
          this.generalRulesForm.patchValue(val, { emitEvent: false });
        }),
        tap(() => (this.error = null)),
        switchMap(filter => {
          let fil = {...filter};
          if (+filter.amount === 0) {
            fil = {...filter, amount: undefined};
          }
          return this.generalRulesService.getGeneralRules(fil).pipe(
            map(result => (this.dataSource = result)),
            catchError(() => (this.error = 'Error: could not load data')),
          )
        }
        ),
        switchMap(() =>
          this.generalRulesService.getFrequencies().pipe(
            map(result => (this.frequencyDropdown = result)),
            catchError(() => (this.error = 'Error: could not load data')),
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

  addNewRule() {
    const dialogRef = this.dialog.open(NewRuleComponent, {
      width: '48rem'
    });

    dialogRef.afterClosed().subscribe(res => {
      if (res !== undefined) {
        this.generalRulesService.postGeneralRule(res).subscribe(() => {
          this.snackBar.customMessage(this.translate.instant('snackbar.success'));
          this.clearFilter();
        });
      }
    });
  }

  editRule(ruleId: number) {
    this.isLoading = true;
    this.generalRulesService.getGeneralRule(ruleId).subscribe(
      res => {
        const dialogRef = this.dialog.open(NewRuleComponent, {
          width: '48rem',
          data: {
            id: ruleId,
            data: res
          }
        });

        dialogRef.afterClosed().subscribe(result => {
          if (result !== undefined) {
            this.generalRulesService
              .putGeneralRule(ruleId, result)
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
    this.generalRulesService.deleteGeneralRule(id).subscribe(res => {
      this.snackBar.customMessage(this.translate.instant('snackbar.deleted'));
      this.clearFilter();
    });
  }
}
