import { Component, OnInit, Inject } from '@angular/core';
import { INewGeneralRuleViewModel } from '../../models/general-rules.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DateAdapter } from '@angular/material/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { GeneralRulesService } from '../../services/general-rules.service';
import { ICurrencyListViewModel, IFrequencyListViewModel } from '../../../general-expenses/models/general-expenses.model';
import { TranslateService } from '@ngx-translate/core';
import { LanguageSetService } from '../../../../core/services/language-set.service';

@Component({
    selector: 'factory-new-rule',
    templateUrl: './new-rule.component.html',
    styleUrls: ['./new-rule.component.scss']
})
export class NewRuleComponent implements OnInit {

    newRule: INewGeneralRuleViewModel;
    newRuleForm: FormGroup;
    submitted = false;
    title = this.translate.instant('GeneralRules.editRule.newTitle');
    currencies: ICurrencyListViewModel[];
    frequencies: IFrequencyListViewModel[];
    lng: string;

    constructor(
        public dialogRef: MatDialogRef<any>,
        private formBuilder: FormBuilder,
        private translate: TranslateService,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private generalRulesService: GeneralRulesService,
        private lngService: LanguageSetService,
        private dateAdapter: DateAdapter<any>
    ) { }

    ngOnInit() {
        this.lng = this.lngService.getLocalLanguageStorage();
        this.dateAdapter.setLocale(this.lng);
        this.newRuleForm = this.formBuilder.group({
            frequencyTypeId: [undefined, Validators.required],
            name: ['', Validators.required],
            startDate: [undefined, Validators.required],
            frequency: [undefined, Validators.required],
            amountValue: [undefined, Validators.required],
            amountCurrencyId: [undefined, Validators.required],
            isFixed: [undefined]
        });
        this.generalRulesService.getCurrencies().subscribe(res => {
            this.currencies = res;
        });
        this.generalRulesService.getFrequencies().subscribe(res => {
            this.frequencies = res;
        });
        if (this.data != null) {
            this.title = this.translate.instant('GeneralRules.editRule.editTitle');
            this.newRuleForm.patchValue(this.data.data);
        }
    }

    cancel(): void {
        this.dialogRef.close();
    }

    get f() { return this.newRuleForm.controls; }

    addNewRule(): void {

        this.submitted = true;

        if (this.newRuleForm.invalid) {
            return;
        }

        this.newRule = {
            name: this.newRuleForm.get('name').value,
            frequencyTypeId: this.newRuleForm.get('frequencyTypeId').value,
            startDate: this.newRuleForm.get('startDate').value,
            frequency: this.newRuleForm.get('frequency').value,
            amountValue: this.newRuleForm.get('amountValue').value,
            amountCurrencyId: this.newRuleForm.get('amountCurrencyId').value,
            isFixed: this.newRuleForm.get('isFixed').value
        };
        this.dialogRef.close(this.newRule);
    }
}
