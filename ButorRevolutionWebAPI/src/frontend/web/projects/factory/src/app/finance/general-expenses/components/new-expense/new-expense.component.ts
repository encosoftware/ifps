import { Component, OnInit, Inject } from '@angular/core';
import { INewGeneralExpenseViewModel, ICurrencyListViewModel, IFrequencyListViewModel } from '../../models/general-expenses.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DateAdapter } from '@angular/material/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { GeneralExpensesService } from '../../services/general-expenses.service';
import { TranslateService } from '@ngx-translate/core';
import { LanguageSetService } from '../../../../core/services/language-set.service';

@Component({
    selector: 'factory-new-expense',
    templateUrl: './new-expense.component.html',
    styleUrls: ['./new-expense.component.scss']
})
export class NewExpenseComponent implements OnInit {

    newExpense: INewGeneralExpenseViewModel;
    newExpenseForm: FormGroup;
    submitted = false;
    title = this.translate.instant('GeneralExpenses.newExpenseComponent.newTitle');
    currencies: ICurrencyListViewModel[];
    frequencies: IFrequencyListViewModel[];
    lng: string;

    constructor(
        public dialogRef: MatDialogRef<any>,
        private formBuilder: FormBuilder,
        private translate: TranslateService,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private generalExpensesService: GeneralExpensesService,
        private lngService: LanguageSetService,
        private dateAdapter: DateAdapter<any>
    ) { }

    ngOnInit() {
        this.lng = this.lngService.getLocalLanguageStorage();
        this.dateAdapter.setLocale(this.lng);
        this.newExpenseForm = this.formBuilder.group({
            frequencyTypeId: [undefined, Validators.required],
            name: ['', Validators.required],
            paymentDate: [undefined, Validators.required],
            interval: [undefined, Validators.required],
            amountValue: [undefined, Validators.required],
            amountCurrencyId: [undefined, Validators.required],
        });
        this.generalExpensesService.getCurrencies().subscribe(res => {
            this.currencies = res;
        });
        this.generalExpensesService.getFrequencies().subscribe(res => {
            this.frequencies = res;
        });
        if (this.data != null) {
            this.title = this.translate.instant('GeneralExpenses.newExpenseComponent.editTitle');
            this.newExpenseForm.patchValue(this.data.data);
        }
    }

    cancel(): void {
        this.dialogRef.close();
    }

    get f() { return this.newExpenseForm.controls; }

    addNewExpense(): void {

        this.submitted = true;

        if (this.newExpenseForm.invalid) {
            return;
        }

        this.newExpense = {
            name: this.newExpenseForm.get('name').value,
            frequencyTypeId: this.newExpenseForm.get('frequencyTypeId').value,
            paymentDate: this.newExpenseForm.get('paymentDate').value,
            interval: this.newExpenseForm.get('interval').value,
            amountValue: this.newExpenseForm.get('amountValue').value,
            amountCurrencyId: this.newExpenseForm.get('amountCurrencyId').value
        };
        this.dialogRef.close(this.newExpense);
    }
}
