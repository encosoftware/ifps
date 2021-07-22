import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild, Optional, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { Subject, Subscription } from 'rxjs';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Store, select } from '@ngrx/store';
import { PagedData } from '../../../../shared/models/paged-data.model';
import { ChangeFilter, DeleteFilter } from '../../store/actions/appliance-list.actions';
import { tap, debounceTime, takeUntil, switchMap, finalize, map, catchError } from 'rxjs/operators';
import { appliancesFilters } from '../../store/selectors/appliance-list.selector';
import { AddNewApplianceComponent } from '../../components/appliances/new-appliance.component';
import { IAppliancesListViewModel } from '../../models/appliences.model';
import { ApplianceService } from '../../services/appliance.service';
import { API_BASE_URL, ClaimPolicyEnum } from '../../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';
import { ISelectItemModel } from '../../../../shared/models/select-items.model';

@Component({
  templateUrl: './appliances.component.html'
})
export class AppliancesComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;

  isLoading = false;
  error: string = null;
  destroy$ = new Subject();
  applianceList: IAppliancesListViewModel[];
  applianceListForm: FormGroup;
  applianceListFormSub: Subscription;

  dataSource: PagedData<IAppliancesListViewModel> = {
    items: [],
    pageIndex: 0,
    pageSize: 10
  };
  claimPolicyEnum = ClaimPolicyEnum;

  appliancesCategories: ISelectItemModel[];

  constructor(
    public dialog: MatDialog,
    private service: ApplianceService,
    private store: Store<any>,
    private formBuilder: FormBuilder,
    private translate: TranslateService,
    @Optional() @Inject(API_BASE_URL) public baseUrl?: string
  ) {}

  ngOnInit() {
    this.loadData();
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

  loadData(): void {
    this.applianceListForm = this.formBuilder.group({
      code: '',
      description: '',
      category: '',
      categoryId: null,
      brand: '',
      hanaCode: null,
      purchasingPrice: null,
      sellPrice: null
    });

    this.applianceListFormSub = this.applianceListForm.valueChanges
      .pipe(
        debounceTime(500),
        tap(values => this.store.dispatch(new ChangeFilter(values))),
        takeUntil(this.destroy$)
      )
      .subscribe();

    this.service.getCategories().subscribe(res => {
      this.appliancesCategories = [];
      res.forEach(x => {
        const temp = {
          options: x.name,
          value: x.id
        };
        this.appliancesCategories.push(temp);
      });
    });

    this.store
      .pipe(
        select(appliancesFilters),
        tap(val =>
          this.applianceListForm.patchValue(val, { emitEvent: false })
        ),
        tap(() => (this.isLoading = true)),
        switchMap(filter =>
          this.service.getApplianceList(filter).pipe(
            map(res => (this.dataSource = res)),
            catchError(() => (this.error = 'Error: could not load data')),
            finalize(() => (this.isLoading = false))
          )
        ),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }

  addNewAppliance() {
    const dialogRef = this.dialog.open(AddNewApplianceComponent, {
      width: '93rem',
      data: {
        isEditable: true
      }
    });

    dialogRef.afterClosed().subscribe(result => this.loadData());
  }

  editAppliance(applienceId: number, editable: boolean) {
    const dialogRef = this.dialog.open(AddNewApplianceComponent, {
      width: '93rem',
      data: {
        isEditable: editable,
        id: applienceId
      }
    });

    dialogRef.afterClosed().subscribe(result => this.loadData());
  }

  clearFilter(): void {
    this.store.dispatch(new DeleteFilter());
  }

  deleteAppliance(id: string) {
    this.service.deleteAppliance(id).subscribe(res => this.loadData());
  }

  get hasError(): boolean {
    return !!this.error;
  }
}
