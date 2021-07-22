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
import { IEdgingMachinesListViewModel, ISupplierDropdownModel } from '../../models/edging_machines.model';
import { EdgingMachineService } from '../../services/edging_machine.service';
import { edgingMachinesFilters } from '../../store/selectors/edging_machine-list.selector';
import { NewEdgingMachineComponent } from '../../components/new-edging_machine/new-edging_machine.component';
import { TranslateService } from '@ngx-translate/core';
import { ChangeFilter, DeleteFilter } from '../../store/actions/edging_machines-list.actions';

@Component({
  selector: 'factory-edging_machines-page',
  templateUrl: './edging_machines-page.component.html',
  styleUrls: ['./edging_machines-page.component.scss']
})
export class EdgingMachinesPageComponent implements OnInit, OnDestroy, AfterViewInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;

  isLoading = false;
  error: string | null = null;
  destroy$ = new Subject();
  edgingMachineForm: FormGroup;
  suppliers: ISupplierDropdownModel[];
  claimPolicyEnum = ClaimPolicyEnum;
  dataSource: PagedData<IEdgingMachinesListViewModel> = { items: [], pageIndex: 0, pageSize: 10 };

  constructor(
    private service: EdgingMachineService,
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
    this.edgingMachineForm = this.formBuilder.group({
      machineName: '',
      softwareVersion: '',
      serialNumber: '',
      code: '',
      yearOfManufacture: null,
      brandId: null
    });

    this.edgingMachineForm.valueChanges.pipe(
      debounceTime(500),
      tap((values) => this.store.dispatch(new ChangeFilter(values))),
      takeUntil(this.destroy$)
    ).subscribe();

    this.store.pipe(
      select(edgingMachinesFilters),
      tap(val => this.edgingMachineForm.patchValue(val, { emitEvent: false })),
      tap(() => this.isLoading = true),
      switchMap((filter) =>
        this.service.getEdgingMachines(filter).pipe(
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

  addNewEdgingMachine(): void {
    const dialogRef = this.dialog.open(NewEdgingMachineComponent, {
      width: '52rem',
      data: {
        id: null
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  editEdgingMachine(id: number): void {
    if (!this.dataSource.items[this.dataSource.items.findIndex(x => x.id === id)]) {
      return;
    }
    const dialogRef = this.dialog.open(NewEdgingMachineComponent, {
      width: '52rem',
      data: {
        id
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  deleteEdgingMachine(id: number) {
    this.service.deleteEdgingMachine(id).subscribe(() => {
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
