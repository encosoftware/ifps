import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { AddNewUserComponent } from '../components/add-new-user/add-new-user.component';
import { FormGroup, FormBuilder } from '@angular/forms';
import { debounceTime, tap, takeUntil, map, switchMap, catchError, finalize } from 'rxjs/operators';
import { Store, select } from '@ngrx/store';
import { ChangeFilter, DeleteFilter } from '../store/actions/user-list.actions';
import { Subject, of } from 'rxjs';
import { userFilters } from '../store/selectors/user-list.selector';
import {
  IUsersListViewModel,
  RolesSelectModel,
  ActivatedFilterModel,
  AddedOnFilterModel,
  ImageDetailsViewModel
} from '../models/users.models';
import { UsersService } from '../services/users.service';
import { PagedData } from '../../../shared/models/paged-data.model';
import { subMonths, subWeeks, subYears, subDays } from 'date-fns';
import { TranslateService } from '@ngx-translate/core';
import { UploadPicService } from '../../../shared/services/upload-pic.service';
import { ClaimPolicyEnum } from '../../../shared/clients';

@Component({
  templateUrl: './users.component.html'
})
export class UsersComponent implements OnInit, OnDestroy, AfterViewInit {

  destroy$ = new Subject();
  claimPolicyEnum = ClaimPolicyEnum;
  userFiltersForm: FormGroup;
  isLoading = false;
  error: string | null = null;
  rolesList: RolesSelectModel[];
  isActivated: ActivatedFilterModel[] = [
    { value: true, label: 'activated' },
    { value: false, label: 'deactivated' }
  ];
  addedOnList: AddedOnFilterModel[] = [
    { name: 'This day', toDate: subDays(new Date(), 1).toString() },
    { name: 'This week', toDate: subWeeks(new Date(), 1).toString() },
    { name: 'This month', toDate: subMonths(new Date(), 1).toString() },
    { name: 'This year', toDate: subYears(new Date(), 1).toString() }
  ];
  basicInfoImage: ImageDetailsViewModel =
    {
      containerName: '',
      fileName: ''
    };
  previewUrl: string | ArrayBuffer;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  dataSource: PagedData<IUsersListViewModel> = { items: [], pageIndex: 0, pageSize: 15 };


  constructor(
    public dialog: MatDialog,
    private formBuilder: FormBuilder,
    private store: Store<any>,
    private userListService: UsersService,
    private translate: TranslateService,
    private PicService: UploadPicService,

  ) { }

  ngAfterViewInit(): void {

    this.paginator.page.pipe(
      tap((val) => this.store.dispatch(new ChangeFilter({ pager: { pageIndex: val.pageIndex, pageSize: val.pageSize } })))
    ).subscribe();

  }

  ngOnInit() {
    this.userFiltersForm = this.formBuilder.group({
      name: '',
      role: '',
      status: '',
      company: '',
      email: '',
      phone: '',
      addedOnTo: '',
    });

    this.userFiltersForm.valueChanges.pipe(
      debounceTime(500),
      tap((values) => this.store.dispatch(new ChangeFilter(values))),
      takeUntil(this.destroy$)
    ).subscribe();

    this.rolesList = [];
    this.userListService.getRoles().subscribe(res => this.rolesList = res);
    this.store.pipe(
      select(userFilters),
      tap(val => this.userFiltersForm.patchValue(val, { emitEvent: false })),
      tap(() => this.isLoading = true),
      switchMap((filter) =>
        this.userListService.getUsers(filter).pipe(
          map(result => this.dataSource = result),
          catchError(() => this.error = 'Error: could not load data'),
          finalize(() => this.isLoading = false)
        ),
      ),
      takeUntil(this.destroy$)
    ).subscribe();
  }

  get hasError(): boolean {
    return !!this.error;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(AddNewUserComponent, {
      width: '48rem',
    });

    dialogRef.afterClosed().subscribe(result => {
    });
  }

  clearFilter() {
    this.store.dispatch(new DeleteFilter());
  }

  toggleActive(data: IUsersListViewModel) {
    if (data.status) {
      this.userListService.putDeactivate(data.id).subscribe(res => this.store.dispatch(new ChangeFilter(this.userFiltersForm.value)));
    } else {
      this.userListService.putAactivate(data.id).subscribe(res => this.store.dispatch(new ChangeFilter(this.userFiltersForm.value)));
    }

  }

  loadImage(containerName: string | undefined, fileName: string | undefined) {
    if (containerName && fileName) {
      this.PicService.getThumbnail(containerName, fileName).pipe(
        tap((resp) => {
          this.previewUrl = resp;
        }),
        catchError((err) => of(err.message))
      ).subscribe();
    }

  }

  deleteButton(id: number) {
    this.userListService.deleteUsers(id).subscribe(res =>
      this.store.dispatch(new ChangeFilter(this.userFiltersForm.value))
    );
  }

}
