import { Component, OnInit, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { Subject } from 'rxjs';
import { IWorkstationsListModel, IWorkstationDropdownModel, IMachineDropdownModel } from '../models/workstations.model';
import { FormGroup, FormBuilder } from '@angular/forms';
import { PagedData } from '../../../shared/models/paged-data.model';
import { WorkstationService } from '../services/workstation.service';
import { Store, select } from '@ngrx/store';
import { tap, debounceTime, takeUntil, switchMap, catchError, map } from 'rxjs/operators';
import { ChangeFilter, DeleteFilter } from '../store/actions/workstations-list.actions';
import { workstationsFilters } from '../store/selectors/workstation-list.selector';
import { NewWorkstationComponent } from '../components/new-workstation/new-workstation.component';
import { ISelectItemModel } from '../../../shared/models/selet-model';
import { AddCameraComponent } from '../components/add-camera/add-camera.component';
import { TranslateService } from '@ngx-translate/core';
import { ClaimPolicyEnum } from '../../../shared/clients';

@Component({
  selector: 'factory-workstations-page',
  templateUrl: './workstations-page.component.html',
  styleUrls: ['./workstations-page.component.scss']
})
export class WorkstationsPageComponent implements OnInit, OnDestroy, AfterViewInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;

  error: string | null = null;
  destroy$ = new Subject();
  workstationList: IWorkstationsListModel[];
  workstationForm: FormGroup;
  types: IWorkstationDropdownModel[];
  statuses: ISelectItemModel[] = [
    { value: true, options: 'Active' },
    { value: false, options: 'Deactive' }
  ];
  claimPolicyEnum = ClaimPolicyEnum;
  machineDd: IMachineDropdownModel[];
  dataSource: PagedData<IWorkstationsListModel> = { items: [], pageIndex: 0, pageSize: 10 };

  constructor(
    public dialog: MatDialog,
    private service: WorkstationService,
    private formBuilder: FormBuilder,
    private translate: TranslateService,
    private store: Store<any>,
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
    this.workstationForm = this.formBuilder.group({
      name: null,
      optimalCrew: null,
      machine: null,
      type: null,
      status: null
    });

    this.service.getWorkstationTypes().subscribe(res => this.types = res);
    this.service.getMachines().subscribe(res => this.machineDd = res);

    this.workstationForm.valueChanges.pipe(
      debounceTime(500),
      tap(values => this.store.dispatch(new ChangeFilter(values))),
      takeUntil(this.destroy$)
    ).subscribe();

    this.store.pipe(
      select(workstationsFilters),
      tap(val => this.workstationForm.patchValue(val, { emitEvent: false })),
      switchMap((filter) => {
        filter.optimalCrew = filter.optimalCrew ? +filter.optimalCrew : undefined;
        return this.service.getWorkstationList(filter).pipe(
          map(res => this.dataSource = res),
          catchError(() => this.error = 'Error: could not load data')
        );
      }),
      takeUntil(this.destroy$)
    ).subscribe();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

  addNewWorkstation(): void {
    const dialogRef = this.dialog.open(NewWorkstationComponent, {
      width: '52rem',
      data: {
        id: null
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  editWorkstation(id: number): void {
    if (!this.dataSource.items[this.dataSource.items.findIndex(x => x.id === id)].status) {
      return;
    }
    const dialogRef = this.dialog.open(NewWorkstationComponent, {
      width: '52rem',
      data: {
        id
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  addCameras(id: number): void {
    if (!this.dataSource.items[this.dataSource.items.findIndex(x => x.id === id)].status) {
      return;
    }
    const dialogRef = this.dialog.open(AddCameraComponent, {
      width: '52rem',
      data: {
        id
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  clearFilter(): void {
    this.store.dispatch(new DeleteFilter());
  }

  activateWorkstation(id: number) {
    this.service.activateWorkstation(id).subscribe(() => this.loadData());
  }

  deleteWorkstation(id: number) {
    this.service.deleteWorkstation(id).subscribe(() => this.loadData());
  }

  get hasError(): boolean {
    return !!this.error;
  }

}
