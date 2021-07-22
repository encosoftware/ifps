import { Component, OnInit, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Store, select } from '@ngrx/store';
import { tap, debounceTime, takeUntil, switchMap, catchError, map } from 'rxjs/operators';
import { IWFUListModel } from '../models/wfurnitureunits.model';
import { WFUService } from '../services/wfurnitureunits.service';
import { ChangeFilter, DeleteFilter } from '../store/actions/wfurnitureunits-list.actions';
import { wFurnitereUnitsFilters } from '../store/selectors/wfurnitureunit-list.selector';
import { NewWFUComponent } from '../components/new-wfurnitureunit/new-wfurnitureunit.component';
import { TranslateService } from '@ngx-translate/core';
import { ClaimPolicyEnum } from '../../../shared/clients';

@Component({
  selector: 'factory-wfurnitureunits-page',
  templateUrl: './wfurnitureunits-page.component.html',
  styleUrls: ['./wfurnitureunits-page.component.scss']
})
export class WFUPageComponent implements OnInit, OnDestroy, AfterViewInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;

  error: string | null = null;
  destroy$ = new Subject();
  wFurnitureUnitsList: IWFUListModel[];
  wFurnitureUnitsForm: FormGroup;
  claimPolicyEnum = ClaimPolicyEnum;
  dataSource: PagedData<IWFUListModel> = { items: [], pageIndex: 0, pageSize: 10 };

  constructor(
    public dialog: MatDialog,
    private service: WFUService,
    private formBuilder: FormBuilder,
    private store: Store<any>,
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
    this.wFurnitureUnitsForm = this.formBuilder.group({
      code: null,
      description: null
    });

    this.wFurnitureUnitsForm.valueChanges.pipe(
      debounceTime(500),
      tap(values => this.store.dispatch(new ChangeFilter(values))),
      takeUntil(this.destroy$)
    ).subscribe();

    this.store.pipe(
      select(wFurnitereUnitsFilters),
      tap(val => this.wFurnitureUnitsForm.patchValue(val, { emitEvent: false })),
      switchMap((filter) => {
        return this.service.getWebshopFurnitureUnits(filter).pipe(
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

  addNewWFurnitureUnit(): void {
    const dialogRef = this.dialog.open(NewWFUComponent, {
      width: '52rem'
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  editWFurnitureUnit(id: number, furnitoreUnitId: string): void {
    const dialogRef = this.dialog.open(NewWFUComponent, {
      width: '52rem',
      data: {
        id,
        furnitoreUnitId,
        title: this.translate.instant('WebshopFurnitureUnits.Labels.editUnit')
      }
    });

    dialogRef.afterClosed().subscribe(() => this.loadData());
  }

  clearFilter(): void {
    this.store.dispatch(new DeleteFilter());
  }

  deleteWFurnitureUnit(id: number) {
    this.service.deleteWFurnitureUnit(id).subscribe(() => this.loadData());
  }

  get hasError(): boolean {
    return !!this.error;
  }
}
