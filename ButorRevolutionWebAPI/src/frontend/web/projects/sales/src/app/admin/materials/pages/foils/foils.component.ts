import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild, Optional, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { AddNewFoilComponent } from '../../components/foils/new-foil.component';
import { FoilService } from '../../services/foil.service';
import { IFoilsListViewModel } from '../../models/foils.model';
import { Subject, Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { PagedData } from '../../../../shared/models/paged-data.model';
import { Store, select } from '@ngrx/store';
import { ChangeFilter, DeleteFilter } from '../../store/actions/foil-list.actions';
import { tap, debounceTime, takeUntil, switchMap, catchError, finalize, map } from 'rxjs/operators';
import { foilsFilters } from '../../store/selectors/foil-list.selector';
import { API_BASE_URL, ClaimPolicyEnum } from '../../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';

@Component({
  templateUrl: './foils.component.html'
})
export class FoilsComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;

  isLoading = false;
  error: string = null;
  destroy$ = new Subject();
  foilList: IFoilsListViewModel[];
  foilListForm: FormGroup;
  foilListFormSub: Subscription;

  dataSource: PagedData<IFoilsListViewModel> = {
    items: [],
    pageIndex: 0,
    pageSize: 10
  };
  claimPolicyEnum = ClaimPolicyEnum;

  constructor(
    public dialog: MatDialog,
    private service: FoilService,
    private store: Store<any>,
    private formBuilder: FormBuilder,
    private translate: TranslateService,
    @Optional() @Inject(API_BASE_URL) public baseUrl?: string
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
    this.loadData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

  loadData(): void {
    this.foilListForm = this.formBuilder.group({
      code: '',
      description: '',
      thickness: '',
      transactionPrice: '',
      purchasingPrice: ''
    });

    this.foilListFormSub = this.foilListForm.valueChanges
      .pipe(
        debounceTime(500),
        tap(values => this.store.dispatch(new ChangeFilter(values))),
        takeUntil(this.destroy$)
      )
      .subscribe();

    this.store
      .pipe(
        select(foilsFilters),
        tap(val => this.foilListForm.patchValue(val, { emitEvent: false })),
        tap(() => (this.isLoading = true)),
        switchMap(filter =>
          this.service.getFoilList(filter).pipe(
            map(res => (this.dataSource = res)),
            catchError(() => (this.error = 'Error: could not load data')),
            finalize(() => (this.isLoading = false))
          )
        ),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }

  addNewFoil() {
    const dialogRef = this.dialog.open(AddNewFoilComponent, {
      width: '93rem',
      data: {
        isEditable: true
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  editFoil(foilId: number, editable: boolean) {
    const dialogRef = this.dialog.open(AddNewFoilComponent, {
      width: '93rem',
      data: {
        isEditable: editable,
        id: foilId
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  clearFilter(): void {
    this.store.dispatch(new DeleteFilter());
  }

  deleteFoil(id: string) {
    this.service.deleteFoil(id).subscribe(() => this.loadData());
  }

  get hasError(): boolean {
    return !!this.error;
  }
}
