import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CargoPreviewComponent } from '../../components/cargo-preview/cargo-preview.component';
import { SupplyService } from '../../services/supply.service';
import { DataService } from '../../../../shared/services/data.service';
import { tap, switchMap, map, finalize, takeUntil, take, catchError } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { ICargoShippingViewModel } from '../../models/cargo-shipping.model';
import { SnackbarService } from 'butor-shared-lib';
import {
  TempCargoDetailsForRequiredMaterialsViewModel,
  ICargoDetailBeforeSaveCargoViewModel,
  IAdditionalsListViewModel,
  IMaterialsListViewModel
} from '../../models/supply-material.models';
import { Store, select } from '@ngrx/store';
import { Subject, of } from 'rxjs';
import { coreLoginT } from '../../../../core/store/selectors/core.selector';

@Component({
  selector: 'factory-create-cargo',
  templateUrl: './create-cargo.component.html',
  styleUrls: ['./create-cargo.component.scss']
})
export class CreateCargoComponent implements OnInit {

  isValid = false;
  isMaterialValid = false;
  isAdditionalsValid = true;
  isMaterialEmpty = false;
  isAdditionalsEmpty = true;
  cargoId?: number;
  submitted = false;
  supplierId?: number;
  bookedById?: number;
  destroy$ = new Subject();
  shipping: ICargoShippingViewModel;

  supplyFiltersForm: FormGroup;
  isLoading = false;
  error: string | null = null;
  newCargo: TempCargoDetailsForRequiredMaterialsViewModel;
  cargoDetailsBeforeSaveCargo: ICargoDetailBeforeSaveCargoViewModel[] = [];

  matNetCost = 0;
  addNetCost = 0;

  constructor(
    public dialog: MatDialog,
    private service: SupplyService,
    private dataService: DataService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: SnackbarService,
    private store: Store<any>,

  ) { }

  ngOnInit() {
    this.cargoId = +this.route.snapshot.paramMap.get('id');
    this.isLoading = true;
    if (this.cargoId === 0) {
      this.store.pipe(
        select(coreLoginT),
        take(1),
        tap(login => {
          this.bookedById = +login.UserId;
          this.supplierId = this.dataService.supplyerID;
        }),
        switchMap((login) =>
          this.dataService.actualIds.pipe(
            switchMap(ids =>
              this.service.sendOrderIds(ids, this.supplierId, this.bookedById).pipe(
                map(orders => {
                  this.cargoDetailsBeforeSaveCargo.push(orders.cargoDetailsBeforeSaveCargo);
                  this.newCargo = orders;
                }),
                catchError(err => of(this.router.navigate(['/supply/orders']))),
                finalize(() => { this.isLoading = false; })
              ))
          )
        ),
        takeUntil(this.destroy$)
      ).subscribe();
    }
  }

  get hasError(): boolean {
    return !!this.error;
  }

  openPreview(): void {
    const dialogRef = this.dialog.open(CargoPreviewComponent, {
      width: '120rem',
      data: {
        id: this.cargoId
      }
    });

    dialogRef.afterClosed().subscribe();
  }

  saveCargo(): void {
    this.submitted = true;
    if (this.isValid) {
      this.isLoading = true;
      if (this.cargoId === 0) {
        this.service.postCargo(this.newCargo, this.bookedById, this.supplierId, this.shipping).subscribe(res => {
          this.isLoading = false;
          this.snackBar.customMessage('Cargo created successfully');
          this.router.navigate(['/supply/cargo']);
        });
      }
    }
  }

  getIsValid(event: boolean): void {
    this.isValid = event;
  }

  getIsMaterialValid(event: boolean): void {
    this.isMaterialValid = event;
  }

  getIsAdditionalsValid(event: boolean): void {
    this.isAdditionalsValid = event;
  }

  getIsMaterialEmpty(event: boolean): void {
    this.isMaterialEmpty = event;

  }

  getIsAdditionalsEmpty(event: boolean): void {
    this.isAdditionalsEmpty = event;
  }

  getShipping(event: ICargoShippingViewModel): void {
    this.shipping = event;
  }

  getMaterial(event: IMaterialsListViewModel[]): void {
    this.newCargo.materials = event;
    this.matNetCost = 0;
    this.newCargo.materials.forEach(res => {
      if (res.materialPackagesSelected && res.orderdAmount) {
        if (res.orderdAmount > 0) {
          this.matNetCost += res.materialPackagesSelected.price.value * res.orderdAmount;
        }
      }
    });
    this.newCargo.cargoDetailsBeforeSaveCargo.netCost.value = this.matNetCost + this.addNetCost;
    this.newCargo.cargoDetailsBeforeSaveCargo.vat.value = this.newCargo.cargoDetailsBeforeSaveCargo.netCost.value * this.newCargo.cargoDetailsBeforeSaveCargo.vatValue;
    this.newCargo.cargoDetailsBeforeSaveCargo.totalGrossCost.value = this.newCargo.cargoDetailsBeforeSaveCargo.netCost.value + this.newCargo.cargoDetailsBeforeSaveCargo.vat.value;
  }

  getAdditionals(event: IAdditionalsListViewModel[]): void {
    this.newCargo.additionals = event;
    this.addNetCost = 0;
    this.newCargo.additionals.forEach(res => {
      if (res.materialPackagesSelected && res.orderdAmount) {
        if (res.orderdAmount > 0) {
          this.addNetCost += res.materialPackagesSelected.price.value * res.orderdAmount;
        }
      }

    });
    this.newCargo.cargoDetailsBeforeSaveCargo.netCost.value = this.matNetCost + this.addNetCost;
    this.newCargo.cargoDetailsBeforeSaveCargo.vat.value = this.newCargo.cargoDetailsBeforeSaveCargo.netCost.value * this.newCargo.cargoDetailsBeforeSaveCargo.vatValue;
    this.newCargo.cargoDetailsBeforeSaveCargo.totalGrossCost.value = this.newCargo.cargoDetailsBeforeSaveCargo.netCost.value + this.newCargo.cargoDetailsBeforeSaveCargo.vat.value;
  }


}
