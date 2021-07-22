import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { AddNewCompanyComponent } from '../components/add-new-company/add-new-company.component';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { ICompanyListModel, ICompanyTypeListModel } from '../models/company.model';
import { CompanyService } from '../services/company.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Store, select } from '@ngrx/store';
import { Subject } from 'rxjs';
import { tap, debounceTime, takeUntil, switchMap, catchError, finalize, map } from 'rxjs/operators';
import { companiesFilters } from '../store/selectors/company-list.selector';
import { ChangeFilter, DeleteFilter } from '../store/actions/company-list.actions';
import { CompanyTypeEnum, ClaimPolicyEnum } from '../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';


@Component({
  templateUrl: './companies.component.html',
})
export class CompaniesComponent implements OnInit, OnDestroy, AfterViewInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;

  isLoading = false;
  error: string | null = null;
  destroy$ = new Subject();
  companyList: ICompanyListModel[];
  companiesForm: FormGroup;

  companyTypesList: ICompanyTypeListModel[];
  selectedCompany: number;
  claimPolicyEnum = ClaimPolicyEnum;


  dataSource: PagedData<ICompanyListModel> = { items: [], pageIndex: 0, pageSize: 10 };

  constructor(
    private router: Router,
    private service: CompanyService,
    public dialog: MatDialog,
    private store: Store<any>,
    private formBuilder: FormBuilder,
    private translate: TranslateService,
  ) { }

  ngAfterViewInit(): void {
    this.paginator.page.pipe(
      tap((val) => this.store.dispatch(new ChangeFilter({ pager: { pageIndex: val.pageIndex, pageSize: val.pageSize } })))
    ).subscribe();
  }

  ngOnInit() {
    this.companiesForm = this.formBuilder.group({
      name: '',
      companyType: '',
      email: '',
      address: ''
    });

    this.service.getCompanyTypes().subscribe(res => {
      this.companyTypesList = res;
      this.companyTypesList.push({ translation: 'None', id: 0, companyType: CompanyTypeEnum.None });
      this.selectedCompany = 0;
    });

    this.companiesForm.valueChanges.pipe(
      debounceTime(500),
      tap((values) => this.store.dispatch(new ChangeFilter(values))),
      takeUntil(this.destroy$)
    ).subscribe();

    this.store.pipe(
      select(companiesFilters),
      tap(val => this.companiesForm.patchValue(val, { emitEvent: false })),
      tap(() => this.isLoading = true),
      switchMap((filter) =>
        this.service.getCompanyList(filter).pipe(
          map(res => this.dataSource = res),
          catchError(() => this.error = 'Error: could not load data'),
          finalize(() => this.isLoading = false)
        )
      ),
      takeUntil(this.destroy$),
    ).subscribe();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

  addNewCompany(): any {
    const dialogRef = this.dialog.open(AddNewCompanyComponent, {
      width: '48rem',
    });

    dialogRef.afterClosed();
  }

  deleteCompany(id: number) {
    this.service.deleteCompany(id).subscribe(res => this.clearFilter());
  }

  clearFilter() {
    this.store.dispatch(new DeleteFilter());
  }

  get hasError(): boolean {
    return !!this.error;
  }

}
