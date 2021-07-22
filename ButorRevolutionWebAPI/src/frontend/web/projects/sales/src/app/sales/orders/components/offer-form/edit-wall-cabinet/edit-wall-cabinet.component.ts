import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IFurnitureUnitDetailsModel, IDecorboardsViewModel } from '../../../models/offer.models';
import { OfferService } from '../../../services/offer.service';
import { Subscription, forkJoin } from 'rxjs';
import { map, finalize } from 'rxjs/operators';
import { IFurnitureUnitCategoryModel } from '../../../../../admin/products/models/products.models';

@Component({
  templateUrl: './edit-wall-cabinet.component.html',
  styleUrls: ['./edit-wall-cabinet.component.scss']
})
export class WallCabinetDialogComponent implements OnInit, OnDestroy {
  isLoading = false;
  furnitureUnit: IFurnitureUnitDetailsModel;
  id: number;
  orderId: string;
  unitId: string;
  subscription$: Subscription;
  categories: IFurnitureUnitCategoryModel[];
  decorboards: IDecorboardsViewModel[];
  foils: IDecorboardsViewModel[];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<any>,
    private service: OfferService
  ) {}

  ngOnInit(): void {
    this.id = this.data.cabinetId;
    this.orderId = this.data.orderId;
    this.unitId = this.data.unitId;
    this.isLoading = true;
    this.subscription$ = forkJoin([
      this.service.getFurnitureUnit(this.orderId, this.id),
      this.service.getDecorboardsDropdown(),
      this.service.getFoilsDropdown(),
      this.service.getFurnitureUnitCategories()
    ])
      .pipe(
        map(([first, retDecor, retFoils, categories]) => {
          this.categories = categories;
          this.furnitureUnit = first;
          this.decorboards = retDecor;
          this.foils = retFoils;
        }),
        finalize(() => (this.isLoading = false))
      )
      .subscribe();
  }

  cancel(): void {
    this.dialogRef.close();
  }

  save(): void {
    this.isLoading = true;
    this.service
      .editFunitureUnit(this.orderId, this.furnitureUnit, this.unitId)
      .subscribe(res => {
        this.isLoading = false;
        this.dialogRef.close();
      });
  }

  getFileName(event): void {}

  getFolderName(event): void {}

  allSideChange(event: IDecorboardsViewModel, id: string): void {
    const temp = this.furnitureUnit.fronts.find(x => x.id === id);
    temp.leftFoilId = event.id;
    temp.rightFoilId = event.id;
    temp.topFoilId = event.id;
    temp.bottomFoilId = event.id;
  }

  ngOnDestroy(): void {
    this.subscription$.unsubscribe();
  }
}
