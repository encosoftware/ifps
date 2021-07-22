import { Component, OnInit, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Store, select } from '@ngrx/store';
import { tap, debounceTime, takeUntil, switchMap, catchError, map } from 'rxjs/operators';
import { IHamburgerMenuModel } from 'butor-shared-lib';
import { IMaterialPackageListModel } from '../models/material-packages.model';
import { MaterialPackageService } from '../services/material-packages.service';
import { ChangeFilter, DeleteFilter } from '../store/actions/material-packages-list.actions';
import { NewMaterialPackageComponent } from '../components/new-material-package/new-material-package.component';
import { materialPackageFilters } from '../store/selectors/material-packages-list.selector';
import { ClaimPolicyEnum } from '../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'factory-material-packages-page',
  templateUrl: './material-packages-page.component.html',
  styleUrls: ['./material-packages-page.component.scss']
})
export class MaterialPackagePageComponent implements OnInit, OnDestroy, AfterViewInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;

  error: string | null = null;
  destroy$ = new Subject();
  materialPackageList: IMaterialPackageListModel[];
  materialPackageForm: FormGroup;
  dataSource: PagedData<IMaterialPackageListModel> = { items: [], pageIndex: 0, pageSize: 10 };
  claimPolicyEnum = ClaimPolicyEnum;
  constructor(
    public dialog: MatDialog,
    private service: MaterialPackageService,
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
    this.materialPackageForm = this.formBuilder.group({
      code: null,
      description: null,
      supplierName: null,
      size: null,
      price: null,
      currency: null
    });

    this.materialPackageForm.valueChanges.pipe(
      debounceTime(500),
      tap(values => this.store.dispatch(new ChangeFilter(values))),
      takeUntil(this.destroy$)
    ).subscribe();

    this.store.pipe(
      select(materialPackageFilters),
      tap(val => this.materialPackageForm.patchValue(val, { emitEvent: false })),
      switchMap((filter) => {
        return this.service.getMaterialPackageList(filter).pipe(
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

  addNewMaterialPackage(): void {
    const dialogRef = this.dialog.open(NewMaterialPackageComponent, {
      width: '52rem',
      data: {
        id: null
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  editMaterialPackage(id: number): void {
    const dialogRef = this.dialog.open(NewMaterialPackageComponent, {
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

  deleteMaterial(id: number) {
    this.service.deleteMaterialPackage(id).subscribe(() => this.loadData());
  }

  get hasError(): boolean {
    return !!this.error;
  }
}
