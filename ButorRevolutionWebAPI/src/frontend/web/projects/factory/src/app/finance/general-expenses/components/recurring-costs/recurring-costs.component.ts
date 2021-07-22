import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IRecurringCostViewModel, ICurrencyListViewModel } from '../../models/general-expenses.model';
import { GeneralExpensesService } from '../../services/general-expenses.service';
import { addMonths } from 'date-fns';

@Component({
    selector: 'factory-recurring-costs',
    templateUrl: './recurring-costs.component.html',
    styleUrls: ['./recurring-costs.component.scss']
})
export class RecurringCostsComponent implements OnInit {

    recurringCostsForm: FormGroup;

    submitted = false;

    fromDate = new Date();
    toDate = addMonths(new Date(), 1);

    currencies: ICurrencyListViewModel[];

    constructor(
        public dialogRef: MatDialogRef<any>,
        private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private generalExpensesService: GeneralExpensesService
    ) { }

    ngOnInit() {
        this.recurringCostsForm = this.formBuilder.group({
            costs: this.formBuilder.array([])
        });
        this.generalExpensesService.getCurrencies().subscribe(res => {
            this.currencies = res;
        });
        if (this.data != null) {
            for (let item of this.data.data) {
                this.addCostRow(item);
            }
        }
    }

    get CostForm() {return this.recurringCostsForm['controls'].costs['controls']; }

    addCostRow(item: IRecurringCostViewModel) {
        let ctrl = this.recurringCostsForm.controls.costs as FormArray;
        ctrl.push(this.formBuilder.group({
            id: item.id,
            name: [item.name, Validators.required],
            amount: [item.amount, Validators.required],
            currency: [item.currency, Validators.required]
        }));
    }

    cancel(): void {
        this.dialogRef.close();
    }

    save(): void {
        this.submitted = true;

        if (this.recurringCostsForm.invalid) {
            return;
        }

        this.dialogRef.close(this.recurringCostsForm.controls.costs.value);
    }
}
