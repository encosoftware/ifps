import { Component, OnInit, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Store, select } from '@ngrx/store';
import { tap, debounceTime, takeUntil, switchMap, catchError, map } from 'rxjs/operators';
import { ICameraListModel } from '../models/cameras.model';
import { CameraService } from '../services/cameras.service';
import { ChangeFilter, DeleteFilter } from '../store/actions/cameras-list.actions';
import { NewCameraComponent } from '../components/new-camera/new-camera.component';
import { IHamburgerMenuModel } from 'butor-shared-lib';
import { camerasFilters } from '../store/selectors/camera-list.selector';
import { ISelectItemModel } from '../../../shared/models/selet-model';
import { TranslateService } from '@ngx-translate/core';
import { ClaimPolicyEnum } from '../../../shared/clients';

@Component({
  selector: 'factory-cameras-page',
  templateUrl: './cameras-page.component.html',
  styleUrls: ['./cameras-page.component.scss']
})
export class CamerasPageComponent implements OnInit, OnDestroy, AfterViewInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;

  error: string | null = null;
  destroy$ = new Subject();
  cameraList: ICameraListModel[];
  cameraForm: FormGroup;
  dataSource: PagedData<ICameraListModel> = { items: [], pageIndex: 0, pageSize: 10 };
  claimPolicyEnum = ClaimPolicyEnum;
  constructor(
    public dialog: MatDialog,
    private service: CameraService,
    private formBuilder: FormBuilder,
    private store: Store<any>,
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
    this.cameraForm = this.formBuilder.group({
      name: null,
      type: null,
      ipAddress: null
    });

    this.cameraForm.valueChanges.pipe(
      debounceTime(500),
      tap(values => this.store.dispatch(new ChangeFilter(values))),
      takeUntil(this.destroy$)
    ).subscribe();

    this.store.pipe(
      select(camerasFilters),
      tap(val => this.cameraForm.patchValue(val, { emitEvent: false })),
      switchMap((filter) => {
        return this.service.getCameraList(filter).pipe(
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

  addNewCamera(): void {
    const dialogRef = this.dialog.open(NewCameraComponent, {
      width: '52rem',
      data: {
        id: null
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  editCamera(id: number): void {
    const dialogRef = this.dialog.open(NewCameraComponent, {
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

  deleteButton(id: number) {
    this.service.deleteCamera(id).subscribe(() => this.loadData());
  }

  get hasError(): boolean {
    return !!this.error;
  }
}
