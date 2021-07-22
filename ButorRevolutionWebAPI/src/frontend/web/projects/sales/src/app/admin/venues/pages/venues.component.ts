import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { PagedData } from '../../../shared/models/paged-data.model';
import { IVenueListViewModel } from '../models/venues.model';
import { Store, select } from '@ngrx/store';
import { VenuesService } from '../services/venues.service';
import { debounceTime, tap, takeUntil, switchMap, map, catchError, finalize } from 'rxjs/operators';
import { ChangeFilter, DeleteFilter } from '../store/actions/venue-list.actions';
import { venueFilters } from '../store/selectors/venue-list.selector';
import { SnackbarService } from 'butor-shared-lib';
import { NewVenueComponent } from '../components/new-venue/new-venue.component';
import { TranslateService } from '@ngx-translate/core';
import { ClaimPolicyEnum } from '../../../shared/clients';

@Component({
  templateUrl: './venues.component.html',
  styleUrls: ['./venues.component.scss']
})
export class VenuesComponent implements OnInit, OnDestroy, AfterViewInit {
  destroy$ = new Subject();

  venueFiltersForm: FormGroup;
  error: string | null = null;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  dataSource: PagedData<IVenueListViewModel> = {
    items: [],
    pageIndex: 0,
    pageSize: 15
  };
  claimPolicyEnum = ClaimPolicyEnum;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private store: Store<any>,
    private venueService: VenuesService,
    public dialog: MatDialog,
    private translate: TranslateService,
    public snackBar: SnackbarService
  ) {}

  ngOnInit() {
    this.venueFiltersForm = this.formBuilder.group({
      name: '',
      rooms: undefined,
      address: '',
      phone: '',
      email: '',
      active: undefined
    });

    this.venueFiltersForm.valueChanges
      .pipe(
        debounceTime(500),
        tap(values => this.store.dispatch(new ChangeFilter(values))),
        takeUntil(this.destroy$)
      )
      .subscribe();

    this.store
      .pipe(
        select(venueFilters),
        tap(val => this.venueFiltersForm.patchValue(val, { emitEvent: false })),
        switchMap(filter =>
          this.venueService.getVenues(filter).pipe(
            map(result => (this.dataSource = result)),
            catchError(() => (this.error = 'Error: could not load data'))
          )
        ),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }

  get hasError(): boolean {
    return !!this.error;
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

  clearFilter() {
    this.store.dispatch(new DeleteFilter());
  }

  buttonDelete(id: number) {
    this.venueService
      .deleteVenue(id)
      .pipe(
        catchError(() => (this.error = 'Something wrong!')),
        finalize(() =>
          this.snackBar.customMessage(this.translate.instant('snackbar.deleted'))
        )
      )
      .subscribe(res =>
        this.store.dispatch(new ChangeFilter(this.venueFiltersForm.value))
      );
  }

  addNewVenue(): void {
    const dialogRef = this.dialog.open(NewVenueComponent, {
      width: '48rem'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.venueService
          .postVenue(result)
          .subscribe(res => this.router.navigate(['/admin/venues', res]));
      }
    });
  }
}
