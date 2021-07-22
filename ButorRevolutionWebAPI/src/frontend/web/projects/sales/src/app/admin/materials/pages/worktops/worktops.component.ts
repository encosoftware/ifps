import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild, Optional, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { AddNewWorktopComponent } from '../../components/worktops/new-worktop.component';
import { IWorktopListViewModel, IGroupingModel } from '../../models/worktops.model';
import { WorkTopService } from '../../services/work-top.service';
import { Subject, Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { PagedData } from '../../../../shared/models/paged-data.model';
import { Store, select } from '@ngrx/store';
import { tap, debounceTime, takeUntil, switchMap, map, catchError, finalize } from 'rxjs/operators';
import { worktopsFilters } from '../../store/selectors/worktop-list.selector';
import { DeleteFilter, ChangeFilter } from '../../store/actions/worktop-list.actions';
import { API_BASE_URL, ClaimPolicyEnum } from '../../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';

@Component({
  templateUrl: './worktops.component.html'
})
export class WorktopsComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;

  error: string = null;
  destroy$ = new Subject();
  worktopList: IWorktopListViewModel[];
  worktopListForm: FormGroup;
  worktopListFormSub: Subscription;

  dataSource: PagedData<IWorktopListViewModel> = {
    items: [],
    pageIndex: 0,
    pageSize: 10
  };

  worktopCategories: IGroupingModel[];
  claimPolicyEnum = ClaimPolicyEnum;

  constructor(
    public dialog: MatDialog,
    private service: WorkTopService,
    private store: Store<any>,
    private formBuilder: FormBuilder,
    private translate: TranslateService,
    @Optional() @Inject(API_BASE_URL) public baseUrl?: string
  ) { }

  ngAfterViewInit(): void {
    this.paginator.page.pipe(
      tap(val => this.store.dispatch(new ChangeFilter({
        pager: { pageIndex: val.pageIndex, pageSize: val.pageSize } })))
    ).subscribe();
  }

  ngOnInit() {
    this.loadData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

  loadData(): void {
    this.worktopListForm = this.formBuilder.group({
      code: '',
      description: '',
      category: '',
      categoryId: null,
      size: '',
      transactionPrice: '',
      purchasingPrice: ''
    });

    this.service.getCategories().subscribe(res => this.worktopCategories = res);

    this.worktopListFormSub = this.worktopListForm.valueChanges
      .pipe(
        debounceTime(500),
        tap(values => this.store.dispatch(new ChangeFilter(values))),
        takeUntil(this.destroy$)
      )
      .subscribe();

    this.store
      .pipe(
        select(worktopsFilters),
        tap(val => this.worktopListForm.patchValue(val, { emitEvent: false })),
        switchMap(filter =>
          this.service.getWorktopList(filter).pipe(
            map(res => (this.dataSource = res)),
            catchError(() => (this.error = 'Error: could not load data'))
          )
        ),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }

  addNewWorktop() {
    const dialogRef = this.dialog.open(AddNewWorktopComponent, {
      width: '93rem',
      data: {
        isEditable: true
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  editWorktop(worktopId: number, editable: boolean) {
    const dialogRef = this.dialog.open(AddNewWorktopComponent, {
      width: '93rem',
      data: {
        isEditable: editable,
        id: worktopId
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  clearFilter() {
    this.store.dispatch(new DeleteFilter());
  }

  deleteWorktop(id: string) {
    this.service.deleteWorktop(id).subscribe(() => this.loadData());
  }

  get hasError(): boolean {
    return !!this.error;
  }
}
