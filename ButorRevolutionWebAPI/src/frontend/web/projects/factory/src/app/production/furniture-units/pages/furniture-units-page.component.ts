import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Store, select } from '@ngrx/store';
import { PagedData } from '../../../shared/models/paged-data.model';
import { FurnitureUnitsService } from '../services/furniture-units.service';
import { map, tap, switchMap, catchError, takeUntil, debounceTime, filter, finalize } from 'rxjs/operators';
import { FurnitureUnitsFilters } from '../store/selectors/furniture-unit-list.selector';
import { Subject } from 'rxjs';
import { ChangeFilter, DeleteFilter } from '../store/actions/furniture-units-list.actions';
import { ISupplyDropModel } from '../../../supply/supply-orders/models/supply-orders.model';
import { IFurnitureUnitDocumentUploadModel, IUploadingFile, IOrderUploadedDocumentItem } from '../models/furniture-unit.model';
import { SnackbarService } from 'butor-shared-lib';

@Component({
    selector: 'factory-furnitureunits-page',
    templateUrl: './furniture-units-page.component.html',
})
export class FurnitureUnitsComponent implements OnInit, OnDestroy, AfterViewInit {
    @ViewChild(MatPaginator) paginator: MatPaginator;

    furnitureUnitsForm: FormGroup;
    categoryDropdown: ISupplyDropModel;
    error: string | null = null;
    destroy$ = new Subject();
    dataSource: PagedData<object> = { items: [], pageIndex: 0, pageSize: 10 };
    files: IUploadingFile[] = [];
    fileResults: IOrderUploadedDocumentItem[] = [];
    currentFileResult: IFurnitureUnitDocumentUploadModel;

    constructor(
        private store: Store<any>,
        private formBuilder: FormBuilder,
        public snackBar: SnackbarService,
        private service: FurnitureUnitsService,
        private furnitureUnitsService: FurnitureUnitsService, ) { }

    ngAfterViewInit(): void {
        this.paginator.page.pipe(
            tap((val) => this.store.dispatch(new ChangeFilter({ pager: { pageIndex: val.pageIndex, pageSize: val.pageSize } })))
        ).subscribe();
    }

    ngOnInit(): void {
        this.furnitureUnitsForm = this.formBuilder.group({
            img: null,
            description: null,
            category: null,
            isUploaded: null,
            code: null
        });

        this.furnitureUnitsForm.valueChanges.pipe(
            debounceTime(500),
            tap(values => this.store.dispatch(new ChangeFilter(values))),
            takeUntil(this.destroy$)
        ).subscribe();

        this.store.pipe(
            select(FurnitureUnitsFilters),
            tap(val => this.furnitureUnitsForm.patchValue(val, { emitEvent: false })),
            switchMap((filter) => {
                return this.service.getFurnitureUnits(filter).pipe(
                    map(res => this.dataSource = res),
                    catchError(() => this.error = 'Error: could not load data')
                );
            }),
            takeUntil(this.destroy$)
        ).subscribe();
    }

    clearFilter(): void {
        this.store.dispatch(new DeleteFilter());
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.unsubscribe();
    }

    get hasError(): boolean {
        return !!this.error;
    }

    onFileSelected(furnitureUnitId, event) {
        if (event.target.files[0]) {
            this.furnitureUnitsService.uploadDocument(event.target.files[0]).pipe(
                map((res) => {
                    if (res) {
                        switch (res.status) {
                            case 'started':
                                break;
                            case 'progress':
                                break;
                            case 'uploaded':
                                let model: IFurnitureUnitDocumentUploadModel = {
                                    furnitureUnitId: furnitureUnitId,
                                    containerName: res.data.item1,
                                    fileName: res.data.item2
                                }
                                return model;
                        }
                    }
                }),
                filter(res => !!res),
                switchMap(data =>
                    this.furnitureUnitsService.uploadXxlFileForCncGeneration(
                        data.furnitureUnitId,
                        data.containerName,
                        data.fileName)
                ),
                finalize(() => {
                    this.snackBar.customMessage('Document uploaded!');
                })
            ).subscribe();
        }
    }
}