import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import {
  IOfferGeneralInformationModel,
  IOfferGeneralCabinetModel,
  IPriceViewModel,
} from '../../../models/offer.models';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { OrdersService } from '../../../services/orders.service';
import { Subscription, forkJoin } from 'rxjs';
import { map, finalize, tap } from 'rxjs/operators';
import { GroupingCategoryListViewModel } from '../../../models/orders';
import { IDropDownViewModel } from '../../../../appointments/models/appointments.model';
import { SpaceSeparatorPipe } from '../../../../../shared/pipes/space-separator.pipe';

@Component({
  selector: 'butor-order-offerform-general',
  templateUrl: './general-information.component.html',
  styleUrls: ['./general-information.component.scss'],
})
export class OrderOfferFormGeneralComponent implements OnInit, OnDestroy {

  @Input() generalInfo: IOfferGeneralInformationModel;
  @Input() topCabinet: IOfferGeneralCabinetModel;
  @Input() baseCabinet: IOfferGeneralCabinetModel;
  @Input() tallCabinet: IOfferGeneralCabinetModel;
  @Input() companies: IDropDownViewModel[];
  @Input() submitted: boolean;
  @Output() generalOutput = new EventEmitter<IOfferGeneralInformationModel>();
  @Output() topOutput = new EventEmitter<IOfferGeneralCabinetModel>();
  @Output() baseOutput = new EventEmitter<IOfferGeneralCabinetModel>();
  @Output() tallOutput = new EventEmitter<IOfferGeneralCabinetModel>();
  isLoading = false;
  subscription$: Subscription;
  isCompany: boolean;
  generalForm: FormGroup;
  currencies: IPriceViewModel[] = [];
  decorboards: GroupingCategoryListViewModel[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private service: OrdersService,
    private separatorPipe: SpaceSeparatorPipe,
  ) { }

  ngOnInit(): void {
    this.isLoading = true;
    this.generalForm = this.formBuilder.group({
      company: null,
      isPrivatePerson: this.generalInfo.privatePerson,
      companyRepresentative: this.generalInfo.companyRepresentative,
      budgetPrice: this.separatorPipe.transform(this.generalInfo.budgetPrice),
      budgetCurrency: this.generalInfo.budgetCurrencyId,
      description: this.generalInfo.description,

      topHeight: this.topCabinet.height,
      topDepth: this.topCabinet.depth,
      topOuterMaterial: this.topCabinet.outerMaterialId,
      topInnerMaterial: this.topCabinet.innerMaterialId,
      topBackMaterial: this.topCabinet.backPanelMaterialId,
      topDoorMaterial: this.topCabinet.doorMaterialId,
      topDescription: this.topCabinet.descrpition,

      baseHeight: this.baseCabinet.height,
      baseDepth: this.baseCabinet.depth,
      baseOuterMaterial: this.baseCabinet.outerMaterialId,
      baseInnerMaterial: this.baseCabinet.innerMaterialId,
      baseBackMaterial: this.baseCabinet.backPanelMaterialId,
      baseDoorMaterial: this.baseCabinet.doorMaterialId,
      baseDescription: this.baseCabinet.descrpition,

      tallHeight: this.tallCabinet.height,
      tallDepth: this.tallCabinet.depth,
      tallOuterMaterial: this.tallCabinet.outerMaterialId,
      tallInnerMaterial: this.tallCabinet.innerMaterialId,
      tallBackMaterial: this.tallCabinet.backPanelMaterialId,
      tallDoorMaterial: this.tallCabinet.doorMaterialId,
      tallDescription: this.tallCabinet.descrpition,
    });

    this.subscription$ = forkJoin([
      this.service.getCurrencies(),
      this.service.getCategoriesForDropdown()
    ]).pipe(
      map(([first, second]) => {
        this.currencies = first;
        this.decorboards = second;
      }),
      finalize(() => {
        this.isLoading = false;
      })
    ).subscribe();

    this.generalForm.controls.isPrivatePerson.valueChanges.pipe(
      tap(() =>
        this.generalForm.controls.companyRepresentative.setValue(!this.generalForm.controls.isPrivatePerson.value, { emitEvent: false }))
    ).subscribe((value) => {
      value ? this.generalForm.controls.company.disable() : this.generalForm.controls.company.enable();
    });

    this.generalForm.controls.companyRepresentative.valueChanges.pipe(
      tap(() =>
        this.generalForm.controls.isPrivatePerson.setValue(!this.generalForm.controls.companyRepresentative.value, { emitEvent: false }))
    ).subscribe((value) => {
      value ? this.generalForm.controls.company.enable() : this.generalForm.controls.company.disable();
    });

    if (!this.generalInfo.privatePerson && !this.generalInfo.companyRepresentative) {
      this.generalForm.controls.isPrivatePerson.setValue(true);
    }

    this.generalForm.controls.budgetPrice.valueChanges.subscribe(res => {
        this.generalForm.controls.budgetPrice.setValue(this.separatorPipe.transform(res),
        { emitEvent: false, emitViewToModelChange: false });
    });

    this.generalForm.valueChanges.pipe(
      tap(() => {
        this.generalInfo = {
          budgetCurrencyId: this.generalForm.controls.budgetCurrency.value,
          budgetPrice: this.toNumber(this.generalForm.controls.budgetPrice.value),
          description: this.generalForm.controls.description.value,
          privatePerson: this.generalForm.controls.isPrivatePerson.value,
          companyRepresentative: this.generalForm.controls.companyRepresentative.value
        };
        this.generalOutput.emit(this.generalInfo);
        this.topCabinet = {
          backPanelMaterialId: this.generalForm.controls.topBackMaterial.value,
          depth: this.generalForm.controls.topDepth.value,
          descrpition: this.generalForm.controls.topDescription.value,
          doorMaterialId: this.generalForm.controls.topDoorMaterial.value,
          height: this.generalForm.controls.topHeight.value,
          innerMaterialId: this.generalForm.controls.topInnerMaterial.value,
          outerMaterialId: this.generalForm.controls.topOuterMaterial.value
        };
        this.topOutput.emit(this.topCabinet);
        this.baseCabinet = {
          backPanelMaterialId: this.generalForm.controls.baseBackMaterial.value,
          depth: this.generalForm.controls.baseDepth.value,
          descrpition: this.generalForm.controls.baseDescription.value,
          doorMaterialId: this.generalForm.controls.baseDoorMaterial.value,
          height: this.generalForm.controls.baseHeight.value,
          innerMaterialId: this.generalForm.controls.baseInnerMaterial.value,
          outerMaterialId: this.generalForm.controls.baseOuterMaterial.value
        };
        this.baseOutput.emit(this.baseCabinet);
        this.tallCabinet = {
          backPanelMaterialId: this.generalForm.controls.tallBackMaterial.value,
          depth: this.generalForm.controls.tallDepth.value,
          descrpition: this.generalForm.controls.tallDescription.value,
          doorMaterialId: this.generalForm.controls.tallDoorMaterial.value,
          height: this.generalForm.controls.tallHeight.value,
          innerMaterialId: this.generalForm.controls.tallInnerMaterial.value,
          outerMaterialId: this.generalForm.controls.tallOuterMaterial.value
        };
        this.tallOutput.emit(this.tallCabinet);
      })
    ).subscribe();
  }

  get f() { return this.generalForm.controls; }

  private toNumber(value: string): number {
    const stringWithoutSpaces = value.replace(/\s/g, '');
    return +stringWithoutSpaces;
  }

  ngOnDestroy(): void {
    this.subscription$.unsubscribe();
  }
}
