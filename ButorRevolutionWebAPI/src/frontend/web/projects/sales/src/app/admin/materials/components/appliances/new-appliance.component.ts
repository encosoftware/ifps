import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ApplianceService } from '../../services/appliance.service';
import { IAppliancesModel, IBrandListModel } from '../../models/appliences.model';
import { FormInputComponent, SnackbarService } from 'butor-shared-lib';
import { forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { IGroupingModel } from '../../models/worktops.model';
import { TranslateService } from '@ngx-translate/core';
import { ISelectItemModel } from '../../../../shared/models/select-items.model';

@Component({
    selector: 'butor-add-new-appliance',
    templateUrl: './new-appliance.component.html'
})
export class AddNewApplianceComponent implements OnInit {

    @ViewChild('codeInput', {static: true}) codeInput: FormInputComponent;

    isLoading = false;
    appliance: IAppliancesModel;
    applianceId: string;
    selectedPurchasingCurrency: number;
    selectedSellingCurrency: number;
    selectedCategory: number;
    selectedBrand: number;
    title = this.translate.instant('Materials.APPLIANCES.Newappliance.title');

    categories: IGroupingModel[] = [];

    sellingCurrencies: ISelectItemModel[];
    purchasingCurrencies: ISelectItemModel[];

    brands: IBrandListModel[];

    applianceForm: FormGroup;
    constructor(
        public dialogRef: MatDialogRef<any>,
        private formBuilder: FormBuilder,
        private service: ApplianceService,
        public snackBar: SnackbarService,
        private translate: TranslateService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) { }

    ngOnInit() {
        this.appliance = {
            id: null,
            code: '',
            brandId: null,
            purchasingCurrencyId: null,
            sellingCurrencyId: null,
            description: '',
            hanaCode: '',
            purchasingPrice: null,
            sellPrice: null,
            categoryId: null,
            picture: {
                containerName: 'MaterialPictures',
                fileName: ''
            }
        };

        forkJoin([
            this.service.getCategories(),
            this.service.getCurrencies(),
            this.service.getCompanies()
        ]).pipe(
            map(([categories, currencies, companies]) => {
                this.categories = categories;
                this.sellingCurrencies = currencies;
                this.purchasingCurrencies = currencies;
                this.brands = companies;
            })
        ).subscribe();

        if (!this.data.isEditable) {
            this.codeInput.setDisabledState(true);
            this.title = this.translate.instant('Materials.APPLIANCES.Newappliance.editTitle');
        }
        this.applianceId = this.data.id;
        if (this.applianceId !== undefined) {
            this.isLoading = true;
            this.service.getAppliance(this.applianceId).subscribe(res => {
                this.appliance = res;
                this.selectedPurchasingCurrency = res.purchasingCurrencyId;
                this.selectedSellingCurrency = res.sellingCurrencyId;
                this.selectedBrand = res.brandId;
                this.selectedCategory = res.categoryId;
            },
                () => { },
                () => this.isLoading = false
            );
        }
        this.applianceForm = this.formBuilder.group({
            code: [this.appliance.code, Validators.required],
            description: [this.appliance.description, Validators.required],
            category: [this.appliance.categoryId, Validators.required],
            brand: [this.appliance.brandId, Validators.required],
            hanaCode: [this.appliance.hanaCode, Validators.required],
            purchasingPrice: [this.appliance.purchasingPrice, Validators.required],
            sellPrice: [this.appliance.sellPrice, Validators.required],
            sellingCurrency: [this.appliance.sellingCurrencyId, Validators.required],
            purchasingCurrency: [this.appliance.purchasingCurrencyId, Validators.required]
        });
    }

    cancel(): void {
        this.dialogRef.close();
    }
    addNewAppliance(): any {
        if (this.applianceForm.invalid) {
            return;
        }
        if (this.applianceId === undefined) {
            this.service.postAppliance(this.appliance).subscribe(() => this.dialogRef.close(),
                () => this.snackBar.customMessage(this.translate.instant('snackbar.error')),
                () => { }
            );
        } else {
            this.service.putAppliance(this.applianceId, this.appliance).subscribe(() => this.dialogRef.close(),
                () => this.snackBar.customMessage(this.translate.instant('snackbar.error')),
                () => { }
            );
        }
    }

    purchasingCurrencyChange(): void {
        this.appliance.purchasingCurrencyId = this.selectedPurchasingCurrency;
    }

    sellingCurrencyChange(): void {
        this.appliance.sellingCurrencyId = this.selectedSellingCurrency;
    }

    categoryChange(): void {
        this.appliance.categoryId = this.selectedCategory;
    }

    brandChange(): void {
        this.appliance.brandId = this.selectedBrand;
    }

    getFileName(event) {
        this.appliance.picture.fileName = event;
    }

    getFolderName(event) {
        this.appliance.picture.containerName = event;
    }

}
