import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild, Optional, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { AddNewAccessoryComponent } from '../../components/accessories/new-accessory.component';
import { IAccessoryListViewModel } from '../../models/accessories.model';
import { AccessoryService } from '../../services/accessory.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Subject, Subscription } from 'rxjs';
import { PagedData } from '../../../../shared/models/paged-data.model';
import { Store, select } from '@ngrx/store';
import { ChangeFilter, DeleteFilter } from '../../store/actions/accessory-list.actions';
import { tap, debounceTime, takeUntil, catchError, finalize, switchMap, map } from 'rxjs/operators';
import { accessoryFilter } from '../../store/selectors/accessory-list.selector';
import { API_BASE_URL, ClaimPolicyEnum } from '../../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';
import { ISelectItemModel } from '../../../../shared/models/select-items.model';

@Component({
  templateUrl: './accessories.component.html'
})
export class AccessoriesComponent implements OnInit, OnDestroy, AfterViewInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;

  error: string = null;
  isLoading = false;
  destroy$ = new Subject();
  accessoryList: IAccessoryListViewModel[];
  accessoryListForm: FormGroup;
  accessoryListFormSub: Subscription;

  dataSource: PagedData<IAccessoryListViewModel> = { items: [], pageIndex: 0, pageSize: 10 };
  claimPolicyEnum = ClaimPolicyEnum;
  optionals: ISelectItemModel[] = [
    { value: true, options: 'true' },
    { value: false, options: 'false' },

  ];
  accessoryCategories: ISelectItemModel[];

  constructor(
    public dialog: MatDialog,
    private service: AccessoryService,
    private store: Store<any>,
    private formBuilder: FormBuilder,
    private translate: TranslateService,
    @Optional() @Inject(API_BASE_URL) public baseUrl?: string
  ) { }

  ngOnInit() {
    this.loadData();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

  ngAfterViewInit(): void {
    this.paginator.page.pipe(
      tap((val) => this.store.dispatch(new ChangeFilter({ pager: { pageIndex: val.pageIndex, pageSize: val.pageSize } })))
    ).subscribe();
  }

  loadData(): void {
    this.accessoryListForm = this.formBuilder.group({
      code: '',
      description: '',
      structurallyOptional: null,
      optMount: null,
      category: '',
      categoryId: null,
      transactionPrice: null,
      purchasingPrice: null
    });

    this.service.getCategories().subscribe(res => this.accessoryCategories = res);

    this.accessoryListFormSub = this.accessoryListForm.valueChanges.pipe(
      debounceTime(500),
      tap((values) => this.store.dispatch(new ChangeFilter(values))),
      takeUntil(this.destroy$)
    ).subscribe();

    this.store.pipe(
      select(accessoryFilter),
      tap(val => this.accessoryListForm.patchValue(val, { emitEvent: false })),
      tap(() => this.isLoading = true),
      switchMap((filter) =>
        this.service.getAccessoryList(filter).pipe(
          map(res => this.dataSource = res),
          catchError(() => this.error = 'Error: could not load data'),
          finalize(() => this.isLoading = false)
        )
      ),
      takeUntil(this.destroy$)
    ).subscribe();
  }

  addNewAccessory() {
    const dialogRef = this.dialog.open(AddNewAccessoryComponent, {
      width: '93rem',
      data: {
        isEditable: true
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  editAccessory(accessoryId: number, editable: boolean) {
    const dialogRef = this.dialog.open(AddNewAccessoryComponent, {
      width: '93rem',
      data: {
        isEditable: editable,
        id: accessoryId
      }
    });

    dialogRef.afterClosed().subscribe(result => this.loadData());
  }

  clearFilter(): void {
    this.store.dispatch(new DeleteFilter());
  }

  deleteAccessorie(id: string) {
    this.service.deleteAccessory(id).subscribe(res => this.loadData());
  }

  get hasError(): boolean {
    return !!this.error;
  }
}
