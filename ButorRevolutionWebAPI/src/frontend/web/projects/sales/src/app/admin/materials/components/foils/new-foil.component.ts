import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IFoilsModel } from '../../models/foils.model';
import { FoilService } from '../../services/foil.service';
import { FormInputComponent, SnackbarService } from 'butor-shared-lib';
import { TranslateService } from '@ngx-translate/core';
import { ISelectItemModel } from '../../../../shared/models/select-items.model';

@Component({
  selector: 'butor-add-new-foil',
  templateUrl: './new-foil.component.html'
})
export class AddNewFoilComponent implements OnInit {

  @ViewChild('codeInput', { static: true }) codeInput: FormInputComponent;

  isLoading = false;
  foil: IFoilsModel;
  foilId: string = null;
  selectedCurrency: number;
  title = this.translate.instant('Materials.Foils.newFoil.title');

  currencies: ISelectItemModel[];

  foilForm: FormGroup;
  constructor(
    public dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private service: FoilService,
    public snackBar: SnackbarService,
    private translate: TranslateService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    this.foil = {
      id: null,
      code: '',
      description: '',
      thickness: null,
      purchasingPrice: null,
      transactionPrice: null,
      currency: '',
      picture: {
        containerName: 'MaterialPictures',
        fileName: ''
      }
    };
    this.service.getCurrencies().subscribe(res => {
      this.currencies = [];
      res.forEach(x => {
        const temp = {
          options: x.options,
          value: x.value
        };
        this.currencies.push(temp);
      });
    });
    if (!this.data.isEditable) {
      this.codeInput.setDisabledState(true);
      this.title = this.translate.instant('Materials.Foils.newFoil.editTitle');
    }
    this.foilId = this.data.id;
    if (this.foilId !== undefined) {
      this.isLoading = true;
      this.service.getFoil(this.foilId).subscribe(res => {
        this.foil = res;
        this.selectedCurrency = res.currencyId;
      },
        () => { },
        () => this.isLoading = false
      );
    }
    this.foilForm = this.formBuilder.group({
      code: [this.foil.code, Validators.required],
      description: [this.foil.description, Validators.required],
      thickness: [this.foil.thickness, Validators.required],
      purchasingPrice: [this.foil.purchasingPrice, Validators.required],
      currency: [this.foil.currency, Validators.required],
      transactionPrice: [this.foil.transactionPrice, Validators.required]
    });
  }

  cancel(): void {
    this.dialogRef.close();
  }

  addNewFoil(): any {
    if (this.foilForm.invalid) {
      return;
    }

    if (this.foilId === undefined) {
      this.service.postFoil(this.foil).subscribe(() => {
        this.snackBar.customMessage(this.translate.instant('snackbar.success'));
        this.dialogRef.close();
      },
        () => this.snackBar.customMessage(this.translate.instant('snackbar.error')),
        () => { }
      );
    } else {
      this.service.putFoil(this.foilId, this.foil).subscribe(() => {
        this.snackBar.customMessage(this.translate.instant('snackbar.saved'));
        this.dialogRef.close();
      },
        () => this.snackBar.customMessage(this.translate.instant('snackbar.error')),
        () => { }
      );
    }
  }

  getFileName(event) {
    this.foil.picture.fileName = event;
  }

  getFolderName(event) {
    this.foil.picture.containerName = event;
  }

  currencyChange(): void {
    this.foil.currencyId = +this.selectedCurrency;
  }

}
