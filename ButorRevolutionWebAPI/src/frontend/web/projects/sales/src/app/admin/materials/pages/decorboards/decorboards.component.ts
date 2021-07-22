import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy, Inject, Optional } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { AddNewDecoroardComponent } from '../../components/decorboards/new-decorboard.component';
import { IDecorboardListViewModel, IGroupingModel } from '../../models/decorboards.model';
import { DecorboardService } from '../../services/decorboard.service';
import { Subject, Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { PagedData } from '../../../../shared/models/paged-data.model';
import { Store, select } from '@ngrx/store';
import { debounceTime, takeUntil, tap, switchMap, catchError, finalize, map } from 'rxjs/operators';
import { ChangeFilter, DeleteFilter } from '../../store/actions/decorboard-list.actions';
import { decorboardFilters } from '../../store/selectors/decorboard-list.selector';
import { API_BASE_URL, ClaimPolicyEnum } from '../../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';

@Component({
  templateUrl: './decorboards.component.html'
})
export class DecorboardsComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;

  error: string = null;
  destroy$ = new Subject();
  decorboardList: IDecorboardListViewModel[];
  decorboardListForm: FormGroup;
  decorboardListFormSub: Subscription;

  dataSource: PagedData<IDecorboardListViewModel> = {
    items: [],
    pageIndex: 0,
    pageSize: 10
  };

  decorboardCategories: IGroupingModel[];
  claimPolicyEnum = ClaimPolicyEnum;

  constructor(
    public dialog: MatDialog,
    private service: DecorboardService,
    private store: Store<any>,
    private formBuilder: FormBuilder,
    private translate: TranslateService,
    @Optional() @Inject(API_BASE_URL) public baseUrl?: string
  ) {}

  ngOnInit() {
    this.loadData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

  ngAfterViewInit(): void {
    this.paginator.page.pipe(
      tap((val) => this.store.dispatch(new ChangeFilter({ pager: { pageIndex: val.pageIndex, pageSize: val.pageSize } })))
    ).subscribe();
  }

  loadData(): void {
    this.decorboardListForm = this.formBuilder.group({
      code: '',
      description: '',
      category: '',
      categoryId: null,
      size: '',
      transactionPrice: '',
      purchasingPrice: ''
    });

    this.service.getCategories().subscribe(res => this.decorboardCategories = res);

    this.decorboardListFormSub = this.decorboardListForm.valueChanges
      .pipe(
        debounceTime(500),
        tap(values => this.store.dispatch(new ChangeFilter(values))),
        takeUntil(this.destroy$)
      )
      .subscribe();

    this.store.pipe(
      select(decorboardFilters),
      tap(val => this.decorboardListForm.patchValue(val, { emitEvent: false })),
      switchMap((filter) =>
        this.service.getDecorboardList(filter).pipe(
          map(res => this.dataSource = res),
          catchError(() => this.error = 'Error: could not load data')
        )
      ),
      takeUntil(this.destroy$)
    ).subscribe();
  }

  addNewDecorboard() {
    const dialogRef = this.dialog.open(AddNewDecoroardComponent, {
      width: '93rem',
      data: {
        isEditable: true
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  editDecorboard(decorboardId: number, editable: boolean) {
    const dialogRef = this.dialog.open(AddNewDecoroardComponent, {
      width: '93rem',
      data: {
        isEditable: editable,
        id: decorboardId
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  clearFilter() {
    this.store.dispatch(new DeleteFilter());
  }

  deleteDecorboard(id: string) {
    this.service.deleteDecorboard(id).subscribe(() => this.loadData());
  }

  get hasError(): boolean {
    return !!this.error;
  }
}
