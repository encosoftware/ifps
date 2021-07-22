import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IDecorboardModel, IGroupingModel } from '../../models/decorboards.model';
import { DecorboardService } from '../../services/decorboard.service';
import { FormInputComponent, SnackbarService } from 'butor-shared-lib';
import { forkJoin } from 'rxjs';
import { map, finalize } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { ISelectItemModel } from '../../../../shared/models/select-items.model';

@Component({
    selector: 'butor-add-new-decorboard',
    templateUrl: './new-decorboard.component.html'
})
export class AddNewDecoroardComponent implements OnInit {

    @ViewChild('codeInput', { static: true }) codeInput: FormInputComponent;

    isLoading = false;
    decorboard: IDecorboardModel;
    decorboardId: string = null;
    selectedCategory: number;
    selectedCurrency: number;
    title = this.translate.instant('Materials.DECORBOARDS.newDecorboard.title');
    currencies: ISelectItemModel[];

    categories: IGroupingModel[];

    decorboardForm: FormGroup;
    constructor(
        public dialogRef: MatDialogRef<any>,
        private formBuilder: FormBuilder,
        private service: DecorboardService,
        public snackBar: SnackbarService,
        private translate: TranslateService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) { }

    ngOnInit() {
        this.decorboard = {
            id: null,
            code: '',
            fiberHeading: null,
            currency: '',
            description: '',
            length: null,
            purchasingPrice: null,
            thickness: null,
            transactionPrice: null,
            width: null,
            category: '',
            categoryId: null,
            currencyId: null,
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
            })
        ).subscribe();

        if (!this.data.isEditable) {
            this.codeInput.setDisabledState(true);
            this.title = this.translate.instant('Materials.DECORBOARDS.newDecorboard.editTitle');
        }
        this.decorboardId = this.data.id;
        if (this.decorboardId !== undefined) {
            this.isLoading = true;
            this.service.getDecorboard(this.decorboardId).subscribe(res => {
                this.decorboard = res;
                this.selectedCategory = res.categoryId;
                this.selectedCurrency = res.currencyId;
                this.decorboardForm.controls.fiberHeadingIsChecked.patchValue(this.decorboard.fiberHeading);
            },
                () => { },
                () => this.isLoading = false
            );
        }
        this.decorboardForm = this.formBuilder.group({
            code: [this.decorboard.code, Validators.required],
            description: [this.decorboard.description, Validators.required],
            category: [this.decorboard.category, Validators.required],
            length: [this.decorboard.length, Validators.required],
            width: [this.decorboard.width, Validators.required],
            currency: [this.decorboard.currency, Validators.required],
            thickness: [this.decorboard.thickness, Validators.required],
            purchasingPrice: [this.decorboard.purchasingPrice, Validators.required],
            transactionPrice: [this.decorboard.transactionPrice, Validators.required],
            fiberHeadingIsChecked: this.decorboard.fiberHeading
        });

        this.decorboardForm.valueChanges.subscribe(() => {
            this.decorboard.fiberHeading = this.decorboardForm.controls.fiberHeadingIsChecked.value;
        });
    }

    cancel(): void {
        this.dialogRef.close();
    }
    addNewDecorboard(): void {
        if (this.decorboardForm.invalid) {
            return;
        }
        if (this.decorboardId === undefined) {
            this.service.postDecorboard(this.decorboard).subscribe(() => {
                this.snackBar.customMessage(this.translate.instant('snackbar.success'));
                this.dialogRef.close();
            },
                () => this.snackBar.customMessage(this.translate.instant('snackbar.error')),
                () => { }
            );
        } else {
            this.service.putDecorboard(this.decorboardId, this.decorboard).subscribe(() => {
                this.snackBar.customMessage(this.translate.instant('snackbar.saved'));
                this.dialogRef.close();
            },
                () => this.snackBar.customMessage(this.translate.instant('snackbar.error')),
                () => { }
            );
        }
    }

    currencyChange(): void {
        this.decorboard.currencyId = +this.selectedCurrency;
    }

    categoryChange(): void {
        this.decorboard.categoryId = +this.selectedCategory;
    }

    getFileName(event) {
        this.decorboard.picture.fileName = event;
    }

    getFolderName(event) {
        this.decorboard.picture.containerName = event;
    }

}
