import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  ViewChild
} from '@angular/core';
import {
  FormCheckboxComponent, NgSelectComponent
} from 'butor-shared-lib';
import { MatDialog } from '@angular/material/dialog';
import {
  IOfferCabinetsModel,
  IOfferAppliancesModel,
  IOfferAccessoriesModel,
  IOfferPricesModel,
  IShippingServiceDetailsModel,
  IServiceDetailsModel,
  IServiceDropdownModel
} from '../../../models/offer.models';
import { WallCabinetDialogComponent } from '../edit-wall-cabinet/edit-wall-cabinet.component';
import { NewProductDialogComponent } from '../new-product/new-product.component';
import { OfferService } from '../../../services/offer.service';
import { ActivatedRoute } from '@angular/router';
import { EditApplianceComponent } from '../edit-appliance/edit-appliance.component';
import { ServiceTypeEnum } from '../../../../../shared/clients';
import { flatMap } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'butor-order-offerform-products',
  templateUrl: './offer-form-products.component.html',
  styleUrls: ['./offer-form-products.component.scss']
})
export class OrderOfferFormProductsComponent implements OnInit {

  @Input() topCabinetDataSource: IOfferCabinetsModel[];
  @Input() baseCabinetDataSource: IOfferCabinetsModel[];
  @Input() tallCabinetDataSource: IOfferCabinetsModel[];
  @Input() appliancesDataSource: IOfferAppliancesModel[];
  @Input() accessoriesDataSource: IOfferAccessoriesModel[];
  @Input() distanceDropDown: IServiceDropdownModel[];
  @Input() prices: IOfferPricesModel;
  @Input() shippingService: IShippingServiceDetailsModel;
  @Input() assemblyService: IServiceDetailsModel;
  @Input() installationService: IServiceDetailsModel;
  @Input() isVatRequired: boolean;
  @Output() reload = new EventEmitter();
  @ViewChild('assemblyCheckbox', {static: true}) assemblyCheckbox: FormCheckboxComponent;
  @ViewChild('installationCheckbox', {static: true}) installationCheckbox: FormCheckboxComponent;
  @ViewChild('shippingCheckbox', {static: true}) shippingCheckbox: FormCheckboxComponent;
  @ViewChild('vatCheckbox', {static: true}) vatCheckbox: FormCheckboxComponent;
  shippingPriceId: number;
  orderId: string;
  isLoading = false;

  constructor(
    public dialog: MatDialog,
    private offerService: OfferService,
    private route: ActivatedRoute,
    private translate: TranslateService
  ) {
  }

  ngOnInit(): void {
    this.orderId = this.route.snapshot.paramMap.get('id');
    this.shippingPriceId = this.shippingService.distanceServiceId ? this.shippingService.distanceServiceId : this.distanceDropDown[0].id;
    this.assemblyCheckbox.setValue(this.assemblyService.isChecked);
    this.installationCheckbox.setValue(this.installationService.isChecked);
    this.shippingCheckbox.setValue(this.shippingService.isChecked);
    this.vatCheckbox.setValue(this.isVatRequired);

    this.verifyCheckboxes();

    this.assemblyCheckbox.change.pipe(
      flatMap((checkboxValue) => this.offerService.checkService(this.orderId, checkboxValue, ServiceTypeEnum.Assembly))
    ).subscribe(() => this.reload.emit('reload'));

    this.installationCheckbox.change.pipe(
      flatMap((checkboxValue) => this.offerService.checkService(this.orderId, checkboxValue, ServiceTypeEnum.Installation))
    ).subscribe(() => this.reload.emit('reload'));

    this.shippingCheckbox.change.pipe(
      flatMap((checkboxValue) => this.offerService.checkService(this.orderId,
        checkboxValue,
        ServiceTypeEnum.Shipping,
        this.shippingPriceId
      ))
    ).subscribe(() => this.reload.emit('reload'));

    this.vatCheckbox.change.pipe(
      flatMap((checkboxValue) => this.offerService.setVat(this.orderId, checkboxValue))
    ).subscribe(() => this.reload.emit('reload'));
  }

  deleteCabinet(unitId: string) {
    this.offerService
      .deleteFurnitureUnit(this.orderId, unitId)
      .subscribe(() => { this.verifyCheckboxes(), this.reload.emit('reload'); });
  }

  deleteAppli(id: number) {
    this.offerService
      .deleteAppliance(this.orderId, id)
      .subscribe(() => { this.verifyCheckboxes(), this.reload.emit('reload'); });
  }

  editWallCabinet(orderId: number, id: number, unitId: number) {
    const dialogRef = this.dialog.open(WallCabinetDialogComponent, {
      width: '152rem',
      panelClass: 'preview-dialog-container',
      data: {
        unitId,
        cabinetId: id,
        orderId
      }
    });

    dialogRef.afterClosed().subscribe(() => this.reload.emit('reload'));
  }

  editApplaince(
    orderId: number,
    id: number,
    unitId: number,
    quanitity: number,
    name: string
  ) {
    const dialogRef = this.dialog.open(EditApplianceComponent, {
      width: '52rem',
      data: {
        unitId,
        applianceMaterialId: id,
        orderId,
        quanitity,
        name
      }
    });

    dialogRef.afterClosed().subscribe(() => this.reload.emit('reload'));
  }

  addTopCabinet() {
    const dialogRef = this.dialog.open(NewProductDialogComponent, {
      width: '52rem',
      data: {
        title: this.translate.instant('Orders.offerForm.products.NewProducts.Titles.Top'),
        id: this.orderId,
        orderedItems: this.topCabinetDataSource,
        getCabinets: () => this.offerService.getTopCabinets(),
        add: (orderId: string, quanitity: number, applianceId) => this.offerService.addFurnitureUnit(orderId, quanitity, applianceId)
      }
    });

    dialogRef.afterClosed().subscribe(() => { this.verifyCheckboxes(), this.reload.emit('reload'); });
  }

  addBaseCabinet() {
    const dialogRef = this.dialog.open(NewProductDialogComponent, {
      width: '52rem',
      data: {
        title: this.translate.instant('Orders.offerForm.products.NewProducts.Titles.Base'),
        orderedItems: this.baseCabinetDataSource,
        id: this.orderId,
        getCabinets: () => this.offerService.getBaseCabinets(),
        add: (orderId: string, quanitity: number, applianceId) => this.offerService.addFurnitureUnit(orderId, quanitity, applianceId)
      }
    });

    dialogRef.afterClosed().subscribe(() => { this.verifyCheckboxes(), this.reload.emit('reload'); });
  }

  addTallCabinet() {
    const dialogRef = this.dialog.open(NewProductDialogComponent, {
      width: '52rem',
      data: {
        title: this.translate.instant('Orders.offerForm.products.NewProducts.Titles.Tall'),
        orderedItems: this.tallCabinetDataSource,
        id: this.orderId,
        getCabinets: () => this.offerService.getTallCabinets(),
        add: (orderId: string, quanitity: number, applianceId) => this.offerService.addFurnitureUnit(orderId, quanitity, applianceId)
      }
    });

    dialogRef.afterClosed().subscribe(() => { this.verifyCheckboxes(), this.reload.emit('reload'); });
  }

  addApplience() {
    const dialogRef = this.dialog.open(NewProductDialogComponent, {
      width: '52rem',
      data: {
        title: this.translate.instant('Orders.offerForm.products.NewProducts.Titles.Appliance'),
        orderedItems: this.appliancesDataSource,
        id: this.orderId,
        getCabinets: () => this.offerService.getAppliances(),
        add: (orderId: string, quanitity: number, applianceId) => this.offerService.addApliance(orderId, quanitity, applianceId)
      }
    });

    dialogRef.afterClosed().subscribe(() => { this.verifyCheckboxes(), this.reload.emit('reload'); });
  }

  changeDistance(event: IServiceDropdownModel): void {
    if (this.shippingCheckbox.value) {
      this.offerService.checkService(this.orderId, this.shippingCheckbox.value, ServiceTypeEnum.Shipping, this.shippingPriceId)
        .subscribe(() => this.reload.emit('reload'));
    }
  }

  setVat(event: IServiceDropdownModel): void {
    if (this.shippingCheckbox.value) {
      this.offerService.checkService(this.orderId, this.shippingCheckbox.value, ServiceTypeEnum.Shipping, this.shippingPriceId)
        .subscribe(() => this.reload.emit('reload'));
    }
  }

  verifyCheckboxes() {
    this.offerService.getOfferDetails(this.orderId).subscribe((res) => {
      // tslint:disable-next-line: max-line-length
      if (res.topCabinetList.length === 0 && res.baseCabinetList.length === 0 && res.tallCabinetList.length === 0 && res.appliancesList.length === 0) {
        this.assemblyCheckbox.setValue(false);
        this.installationCheckbox.setValue(false);
        this.vatCheckbox.setValue(false);

        this.assemblyCheckbox.setDisabledState(true);
        this.installationCheckbox.setDisabledState(true);
        this.vatCheckbox.setDisabledState(true);
      } else {
        this.assemblyCheckbox.setDisabledState(false);
        this.installationCheckbox.setDisabledState(false);
        this.vatCheckbox.setDisabledState(false);
      }
    });
  }
}
