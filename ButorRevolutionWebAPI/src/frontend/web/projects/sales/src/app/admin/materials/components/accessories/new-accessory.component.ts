import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IAccessoryModel } from '../../models/accessories.model';
import { AccessoryService } from '../../services/accessory.service';
import { SnackbarService } from 'butor-shared-lib';
import { forkJoin } from 'rxjs';
import { map, finalize } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { ISelectItemModel } from '../../../../shared/models/select-items.model';

@Component({
  selector: 'butor-add-new-accessory',
  templateUrl: './new-accessory.component.html'
})
export class AddNewAccessoryComponent implements OnInit {

  isLoading = false;
  accessoryId: string;
  accessory: IAccessoryModel;
  selectedCategory: number;
  selectedCurrency: number;
  currencies: ISelectItemModel[];
  showPictureComponent = false;
  title = this.translate.instant('Materials.ACCESSORIES.NEWACCESSORY.title');

  categories: ISelectItemModel[];

  accessoryForm: FormGroup;
  constructor(
    public dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private service: AccessoryService,
    public snackBar: SnackbarService,
    private translate: TranslateService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    this.isLoading = true;
    this.accessory = {
      id: null,
      code: '',
      description: '',
      currency: '',
      optMount: null,
      structurallyOptional: null,
      purchasingPrice: null,
      transactionPrice: null,
      category: '',
      picture: {
        containerName: 'MaterialPictures',
        fileName: undefined
      }
    };

    this.accessoryForm = this.formBuilder.group({
      code: [this.accessory.code, Validators.required],
      description: [this.accessory.description, Validators.required],
      category: [this.accessory.categoryId, Validators.required],
      purchasingPrice: [this.accessory.purchasingPrice, Validators.required],
      transactionPrice: [this.accessory.transactionPrice, Validators.required],
      currency: [this.accessory.currency, Validators.required],
      optionalIsChecked: this.accessory.structurallyOptional,
      assemblyIsChecked: this.accessory.optMount
    });

    forkJoin([
      this.service.getCategories(),
      this.service.getCurrencies()
    ]).pipe(
      map(([categories, currencies]) => {
        this.categories = categories;
        this.currencies = currencies;
      }),
      finalize(() => this.isLoading = false)
    ).subscribe();

    this.accessoryId = this.data.id;
    if (!this.data.isEditable) {
      this.accessoryForm.controls.code.disable();
      this.title = this.translate.instant('Materials.ACCESSORIES.NEWACCESSORY.editTitle');
    }
    if (this.accessoryId !== undefined) {
      this.service.getAccessory(this.accessoryId).subscribe(res => {
        this.accessory = res;
        this.selectedCategory = res.categoryId;
        this.selectedCurrency = res.currencyId;
        this.accessoryForm.controls.optionalIsChecked.setValue(this.accessory.structurallyOptional);
        this.accessoryForm.controls.assemblyIsChecked.setValue(this.accessory.optMount);
        this.accessoryForm.controls.optionalIsChecked.disable();
        this.accessoryForm.controls.assemblyIsChecked.disable();
        this.accessoryForm.controls.category.setValue(this.accessory.categoryId);
        this.accessoryForm.controls.code.setValue(this.accessory.code);
        this.accessoryForm.controls.description.setValue(this.accessory.description);
        this.accessoryForm.controls.purchasingPrice.setValue(this.accessory.purchasingPrice);
        this.accessoryForm.controls.currency.setValue(this.accessory.currencyId);
        this.accessoryForm.controls.transactionPrice.setValue(this.accessory.transactionPrice);
      },
        () => { },
        () => {
          this.isLoading = false;
          this.showPictureComponent = true;
        }
      );
    } else {
      this.showPictureComponent = true;
    }
    this.accessoryForm.get('assemblyIsChecked').valueChanges.subscribe(res => this.accessory.optMount = res);
    this.accessoryForm.get('optionalIsChecked').valueChanges.subscribe(res => this.accessory.structurallyOptional = res);
  }

  cancel(): void {
    this.dialogRef.close();
  }
  addNewAccessory(): any {

    if (this.accessoryForm.invalid) {
      return;
    }

    this.accessory.categoryId = this.accessoryForm.controls.category.value;
    this.accessory.code = this.accessoryForm.controls.code.value;
    this.accessory.description = this.accessoryForm.controls.description.value;
    this.accessory.purchasingPrice = this.accessoryForm.controls.purchasingPrice.value;
    this.accessory.currencyId = this.accessoryForm.controls.currency.value;
    this.accessory.transactionPrice = this.accessoryForm.controls.transactionPrice.value;
    if (this.accessoryId === undefined) {
      this.service.postAccessory(this.accessory).subscribe(() => {
        this.snackBar.customMessage(this.translate.instant('snackbar.success'));
        this.dialogRef.close();
      },
        () => this.snackBar.customMessage(this.translate.instant('snackbar.error')),
        () => { }
      );
    } else {
      this.service.putAccessory(this.accessoryId, this.accessory).subscribe(() => {
        this.snackBar.customMessage(this.translate.instant('snackbar.saved'));
        this.dialogRef.close();
      },
      () => this.snackBar.customMessage(this.translate.instant('snackbar.error')),
        () => { }
      );
    }
  }

  currencyChange(): void {
    this.accessory.currencyId = +this.selectedCurrency;
  }

  categoryChange(): void {
    this.accessory.categoryId = +this.selectedCategory;
  }

  getFileName(event): void {
    this.accessory.picture.fileName = event;
  }

  getFolderName(event): void {
    this.accessory.picture.containerName = event;
  }

}
