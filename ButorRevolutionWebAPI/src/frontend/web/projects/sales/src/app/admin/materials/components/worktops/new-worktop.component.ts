import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IWorktopModel, IGroupingModel } from '../../models/worktops.model';
import { WorkTopService } from '../../services/work-top.service';
import { FormInputComponent, SnackbarService } from 'butor-shared-lib';
import { forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { ISelectItemModel } from '../../../../shared/models/select-items.model';

@Component({
  selector: 'butor-add-new-worktop',
  templateUrl: './new-worktop.component.html'
})
export class AddNewWorktopComponent implements OnInit {

  @ViewChild('codeInput', { static: true }) codeInput: FormInputComponent;

  isLoading = false;
  worktop: IWorktopModel;
  worktopId: string = null;
  selectedCategory: number;
  selectedCurrency: number;
  currencies: ISelectItemModel[];
  categories: IGroupingModel[];
  worktopForm: FormGroup;
  title = this.translate.instant('Materials.WorkTop.newWorktop.title');

  constructor(
    public dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private service: WorkTopService,
    public snackBar: SnackbarService,
    private translate: TranslateService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    this.worktop = {
      id: null,
      fiberHeading: null,
      code: '',
      description: '',
      currency: '',
      length: null,
      purchasingPrice: null,
      thickness: null,
      transactionPrice: null,
      width: null,
      category: '',
      categoryId: null,
      picture: {
        containerName: 'MaterialPictures',
        fileName: ''
      }
    };
    forkJoin([
      this.service.getCategories(),
      this.service.getCurrencies()
    ]).pipe(
      map(([categories, currencies]) => {
        this.categories = categories;
        this.currencies = currencies;
      })).subscribe();
    if (!this.data.isEditable) {
      this.codeInput.setDisabledState(true);
      this.title = this.translate.instant('Materials.WorkTop.newWorktop.editTitle');
    }
    this.worktopId = this.data.id;
    if (this.worktopId !== undefined) {
      this.isLoading = true;
      this.service.getWorktop(this.worktopId).subscribe(res => {
        this.worktop = res;
        this.selectedCategory = res.categoryId;
        this.selectedCurrency = res.currencyId;
        this.worktopForm.controls.fiberHeadingIsChecked.patchValue(this.worktop.fiberHeading);
      },
        () => { },
        () => this.isLoading = false
      );
    }

    this.worktopForm = this.formBuilder.group({
      code: [this.worktop.code, Validators.required],
      description: [this.worktop.description, Validators.required],
      category: [this.worktop.category, Validators.required],
      length: [this.worktop.length, Validators.required],
      width: [this.worktop.width, Validators.required],
      currency: [this.worktop.currency, Validators.required],
      thickness: [this.worktop.thickness, Validators.required],
      purchasingPrice: [this.worktop.purchasingPrice, Validators.required],
      transactionPrice: [this.worktop.transactionPrice, Validators.required],
      fiberHeadingIsChecked: this.worktop.fiberHeading
    });

    this.worktopForm.valueChanges.subscribe(() => {
      this.worktop.fiberHeading = this.worktopForm.controls.fiberHeadingIsChecked.value;
    });
  }

  cancel(): void {
    this.dialogRef.close();
  }
  addNewWroktop(): any {

    if (this.worktopForm.invalid) {
      return;
    }

    if (this.worktopId === undefined) {
      this.service.postWorktop(this.worktop).subscribe(() => {
        this.snackBar.customMessage(this.translate.instant('snackbar.success'));
        this.dialogRef.close();
      },
        () => this.snackBar.customMessage(this.translate.instant('snackbar.error')),
        () => { }
      );
    } else {
      this.service.putWorktop(this.worktop.id, this.worktop).subscribe(() => {
        this.snackBar.customMessage(this.translate.instant('snackbar.saved'));
        this.dialogRef.close();
      },
        () => this.snackBar.customMessage(this.translate.instant('snackbar.error')),
        () => { }
      );
    }
  }

  currencyChange(): void {
    this.worktop.currencyId = +this.selectedCurrency;
  }

  categoryChange(): void {
    this.worktop.categoryId = +this.selectedCategory;
  }

  getFileName(event) {
    this.worktop.picture.fileName = event;
  }

  getFolderName(event) {
    this.worktop.picture.containerName = event;
  }

}
