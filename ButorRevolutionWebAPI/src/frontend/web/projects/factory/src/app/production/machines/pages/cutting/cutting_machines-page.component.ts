import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { SnackbarService } from 'butor-shared-lib';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { tap, debounceTime, takeUntil, switchMap, map, catchError, finalize } from 'rxjs/operators';
import { Store, select } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { PagedData } from '../../../../shared/models/paged-data.model';
import { ClaimPolicyEnum } from '../../../../shared/clients';
import { ChangeFilter, DeleteFilter } from '../../store/actions/cutting_machines-list.actions';
import { ICuttingMachinesListViewModel, ISupplierDropdownModel } from '../../models/cutting_machines.model';
import { CuttingMachineService } from '../../services/cutting_machine.service';
import { cuttingMachinesFilters } from '../../store/selectors/cutting_machine-list.selector';
import { NewCuttingMachineComponent } from '../../components/new-cutting_machine/new-cutting_machine.component';

@Component({
  selector: 'factory-cutting_machines-page',
  templateUrl: './cutting_machines-page.component.html',
  styleUrls: ['./cutting_machines-page.component.scss']
})
export class CuttingMachinesPageComponent implements OnInit, OnDestroy, AfterViewInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;

  isLoading = false;
  error: string | null = null;
  destroy$ = new Subject();
  cuttingMachineForm: FormGroup;
  suppliers: ISupplierDropdownModel[];
  claimPolicyEnum = ClaimPolicyEnum;
  dataSource: PagedData<ICuttingMachinesListViewModel> = { items: [], pageIndex: 0, pageSize: 10 };

  constructor(
    private service: CuttingMachineService,
    private formBuilder: FormBuilder,
    private store: Store<any>,
    private snackBar: SnackbarService,
    public dialog: MatDialog,
    private translate: TranslateService,
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
    this.cuttingMachineForm = this.formBuilder.group({
      machineName: '',
      softwareVersion: '',
      serialNumber: '',
      code: '',
      yearOfManufacture: null,
      brandId: null
    });

    this.cuttingMachineForm.valueChanges.pipe(
      debounceTime(500),
      tap((values) => this.store.dispatch(new ChangeFilter(values))),
      takeUntil(this.destroy$)
    ).subscribe();

    this.store.pipe(
      select(cuttingMachinesFilters),
      tap(val => this.cuttingMachineForm.patchValue(val, { emitEvent: false })),
      tap(() => this.isLoading = true),
      switchMap((filter) =>
        this.service.getCuttingMachines(filter).pipe(
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

  addNewCuttingMachine(): void {
    const dialogRef = this.dialog.open(NewCuttingMachineComponent, {
      width: '52rem',
      data: {
        id: null
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  editCuttingMachine(id: number): void {
    if (!this.dataSource.items[this.dataSource.items.findIndex(x => x.id === id)]) {
      return;
    }
    const dialogRef = this.dialog.open(NewCuttingMachineComponent, {
      width: '52rem',
      data: {
        id
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  deleteCuttingMachine(id: number) {
    this.service.deleteCuttingMachine(id).subscribe(() => {
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
