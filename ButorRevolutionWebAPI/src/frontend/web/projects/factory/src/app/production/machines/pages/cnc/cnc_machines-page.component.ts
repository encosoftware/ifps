import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { SnackbarService } from 'butor-shared-lib';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { tap, debounceTime, takeUntil, switchMap, map, catchError, finalize } from 'rxjs/operators';
import { Store, select } from '@ngrx/store';
import { PagedData } from '../../../../shared/models/paged-data.model';
import { ClaimPolicyEnum } from '../../../../shared/clients';
import { CncMachineService } from '../../services/cnc_machine.service';
import { ICncMachinesListViewModel, ISupplierDropdownModel } from '../../models/cnc_machines.model';
import { cncMachinesFilters } from '../../store/selectors/cnc_machine-list.selector';
import { NewCncMachineComponent } from '../../components/new-cnc_machine/new-cnc_machine.component';
import { TranslateService } from '@ngx-translate/core';
import { ChangeFilter, DeleteFilter } from '../../store/actions/cnc_machines-list.actions';

@Component({
  selector: 'factory-cnc_machines-page',
  templateUrl: './cnc_machines-page.component.html',
  styleUrls: ['./cnc_machines-page.component.scss']
})
export class CncMachinesPageComponent implements OnInit, OnDestroy, AfterViewInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;

  isLoading = false;
  error: string | null = null;
  destroy$ = new Subject();
  cncMachineForm: FormGroup;
  suppliers: ISupplierDropdownModel[];
  claimPolicyEnum = ClaimPolicyEnum;
  dataSource: PagedData<ICncMachinesListViewModel> = { items: [], pageIndex: 0, pageSize: 10 };
  concreteMachineTypeList = [];

  constructor(
    private service: CncMachineService,
    private formBuilder: FormBuilder,
    private store: Store<any>,
    private snackBar: SnackbarService,
    public dialog: MatDialog,
    private translate: TranslateService
  ) { }

  ngAfterViewInit(): void {
    this.paginator.page.pipe(
      tap((val) => this.store.dispatch(new ChangeFilter({ pager: { pageIndex: val.pageIndex, pageSize: val.pageSize } })))
    ).subscribe();
  }

  ngOnInit() {
    this.loadData();
  }

  loadData(): void {
    this.isLoading = true;
    this.service.getSuppliers().subscribe(res => this.suppliers = res);
    this.cncMachineForm = this.formBuilder.group({
      machineName: '',
      softwareVersion: '',
      serialNumber: '',
      code: '',
      yearOfManufacture: null,
      brandId: null
    });

    this.cncMachineForm.valueChanges.pipe(
      debounceTime(500),
      tap((values) => this.store.dispatch(new ChangeFilter(values))),
      takeUntil(this.destroy$)
    ).subscribe();

    this.store.pipe(
      select(cncMachinesFilters),
      tap(val => this.cncMachineForm.patchValue(val, { emitEvent: false })),
      tap(() => this.isLoading = true),
      switchMap((filter) =>
        this.service.getCncMachines(filter).pipe(
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

  addNewCncMachine(): void {
    const dialogRef = this.dialog.open(NewCncMachineComponent, {
      width: '52rem',
      data: {
        id: null
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  editCncMachine(id: number): void {
    if (!this.dataSource.items[this.dataSource.items.findIndex(x => x.id === id)]) {
      return;
    }
    const dialogRef = this.dialog.open(NewCncMachineComponent, {
      width: '52rem',
      data: {
        id
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  deleteCncMachine(id: number) {
    this.service.deleteCncMachine(id).subscribe(res => {
      this.snackBar.customMessage(this.translate.instant('snackbar.deleted'));
      this.clearFilter();
    });
  }

  clearFilter(): void {
    this.store.dispatch(new DeleteFilter());
  }

  get hasError(): boolean {
    return !!this.error;
  }
}
