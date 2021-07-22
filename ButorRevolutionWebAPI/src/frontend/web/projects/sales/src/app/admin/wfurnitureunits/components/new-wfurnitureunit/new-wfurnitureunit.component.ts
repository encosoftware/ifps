import { Component, OnInit, Inject, AfterViewInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { WFUService } from '../../services/wfurnitureunits.service';
import { IFurnitureUnitDropdownModel, FurnitureUnitForWebshopViewModel, ICurrenciesDropdownModel, IWFUDetailsModel } from '../../models/wfurnitureunits.model';
import { map, tap, mergeMap } from 'rxjs/operators';
import { IPictureModel } from '../../../materials/models/decorboards.model';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'factory-new-wfurnitureunit',
  templateUrl: './new-wfurnitureunit.component.html',
  styleUrls: ['./new-wfurnitureunit.component.scss']
})
export class NewWFUComponent implements OnInit, AfterViewInit {

  title = this.translate.instant('WebshopFurnitureUnits.Labels.newUnit');
  id: number;
  furnitureUnitId: string;
  webshopFurnitureUnitForm: FormGroup;
  submitted = false;
  furnitureUnits: IFurnitureUnitDropdownModel[];
  currencies: ICurrenciesDropdownModel[];
  details: FurnitureUnitForWebshopViewModel;
  images: IPictureModel[];
  constructor(
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private service: WFUService,
    private translate: TranslateService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    this.webshopFurnitureUnitForm = this.formBuilder.group({
      furnitureUnitId: [null, Validators.required],
      code: [null],
      description: [null],
      priceValue: [null, Validators.required],
      priceCurrency: [1, Validators.required],
    });
    if (this.data) {
      this.id = this.data.id;
      this.furnitureUnitId = this.data.furnitoreUnitId;
      this.title = this.data.title;
    }
    this.service.getBaseFurnitureUnits().subscribe(res => {
      this.furnitureUnits = res;
      if (this.furnitureUnitId) {
        this.webshopFurnitureUnitForm.controls.furnitureUnitId.setValue(this.furnitureUnitId);
      }
    });
    this.service.getCurrencies().subscribe(res => (this.currencies = res));

  }

  ngAfterViewInit(): void {
    this.webshopFurnitureUnitForm.controls.furnitureUnitId.valueChanges.pipe(
      map(res => this.furnitureUnitId = res),
      mergeMap((id: string) => {
        let furnitoreUnitDetails;
        if (!this.id) {
          furnitoreUnitDetails = this.service.getFurnitureUnitForWFU(id).pipe(
            map(res => this.details = ({
              code: res.code ? res.code : undefined,
              description: res.description ? res.description : undefined,
              imageDetailsDto: res.imageDetailsDto ? res.imageDetailsDto.map(resp => ({
                containerName: resp.containerName ? resp.containerName : undefined,
                fileName: resp.fileName ? resp.fileName : undefined,
              })) : undefined
            })
            ),
            tap(res => {
              this.webshopFurnitureUnitForm.controls.code.setValue(res.code);
              this.webshopFurnitureUnitForm.controls.description.setValue(res.description);
            })
          )
        } else {
          furnitoreUnitDetails = this.service.getWebshopFurnitureUnitById(this.id).pipe(
            map(dto => this.details = ({
              furnitureUnitId: dto.furnitureUnitId,
              imageDetailsDto: dto.images ? dto.images.map(res => ({
                containerName: res.containerName,
                fileName: res.fileName
              })
              ) : undefined,
              price: dto.price ? ({
                value: dto.price.value,
                currencyId: dto.price.currencyId,
                currency: dto.price.currency
              }) : undefined
            })
            ),
            tap((res) => {
              this.webshopFurnitureUnitForm.controls.priceValue.setValue(res.price.value);
              this.webshopFurnitureUnitForm.controls.priceCurrency.setValue(res.price.currencyId);
            }),
          );
        }
        return furnitoreUnitDetails;
      }
      )
    ).subscribe(res => this.images = this.details.imageDetailsDto);
  }

  get f() { return this.webshopFurnitureUnitForm.controls; }

  onSubmit() {
    if (!this.webshopFurnitureUnitForm.invalid) {
      const detailsModel: IWFUDetailsModel = ({
        furnitureUnitId: this.webshopFurnitureUnitForm.controls.furnitureUnitId.value,
        images: this.images,
        price: ({
          value: this.webshopFurnitureUnitForm.controls.priceValue.value,
          currencyId: this.webshopFurnitureUnitForm.controls.priceCurrency.value,
        })
      });
      if (this.id) {
        this.service.updateWebshopFurnitureUnit(this.id, detailsModel).subscribe(res => this.dialogRef.close());
      } else {
        this.service.createWebshopFurnitureUnit(detailsModel).subscribe(res => this.dialogRef.close());
      }
    }
  }

  getFileName(images: IPictureModel[]) {
    this.images = images.map(res => ({
      containerName: res.containerName,
      fileName: res.fileName
    }));
  }

  cancel(): void {
    this.dialogRef.close();
  }

}