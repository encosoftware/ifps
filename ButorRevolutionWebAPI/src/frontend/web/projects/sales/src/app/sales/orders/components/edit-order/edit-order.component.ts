import { Component, OnInit, Inject } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IEditOrderViewModel } from '../../models/orders';
import { addDays } from 'date-fns';
import { debounceTime, tap, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { OrdersService } from '../../services/orders.service';
import { LanguageSetService } from '../../../../core/services/language-set.service';

@Component({
    templateUrl: './edit-order.component.html',
    styleUrls: ['./edit-order.component.scss']
})
export class EditOrderComponent implements OnInit {

    customers = [];

    sales = [];

    countries = [];

    statuses = [];

    submitted = false;

    order: IEditOrderViewModel;

    destroy$ = new Subject();

    editOrderForm: FormGroup;

    minDate = new Date();
    constructor(
        private dialogRef: MatDialogRef<any>,
        private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private orderService: OrdersService,
        private lngService: LanguageSetService,
        private dateAdapter: DateAdapter<any>
    ) { }

    ngOnInit(): void {
        this.dateAdapter.setLocale(this.lngService.getLocalLanguageStorage());
        this.editOrderForm = this.formBuilder.group({
            customerId: [undefined, Validators.required],
            customerName: '',
            countryId: [undefined, Validators.required],
            postCode: [undefined, Validators.required],
            city: ['', Validators.required],
            address: ['', Validators.required],
            salesId: [undefined, Validators.required],
            deadline: [addDays(new Date(), 4), Validators.required],
            responsibleId: [undefined, Validators.required],
            currentStatusId: [undefined, Validators.required],
            statusDeadline: [addDays(new Date(), 1), Validators.required],
            orderId: undefined,
            orderName: ''
        });
        this.editOrderForm.patchValue(this.data.order);
        this.countries = this.data.countries;
        this.sales = this.data.sales;
        this.statuses = this.data.statuses;
        this.customers.push({
            id: this.editOrderForm.get('customerId').value,
            name: this.editOrderForm.get('customerName').value
        });
    }

    get f() { return this.editOrderForm.controls; }

    cancel(): void {
        this.dialogRef.close();
    }

    searchCustomers(text: string) {
        this.orderService.getCustomers(text, 10).pipe(
          debounceTime(500),
          tap(res => this.customers = res),
          takeUntil(this.destroy$)
        ).subscribe();
      }

    save(): any {
        this.submitted = true;

        if (this.editOrderForm.invalid) {
            return;
        }

        this.order = {
            customerId: this.editOrderForm.controls['customerId'].value,
            orderId: this.editOrderForm.controls['orderId'].value,
            orderName: this.editOrderForm.controls['orderName'].value,
            salesId: this.editOrderForm.controls['salesId'].value,
            responsibleId: this.editOrderForm.controls['responsibleId'].value,
            deadline: this.editOrderForm.controls['deadline'].value,
            countryId: this.editOrderForm.controls['countryId'].value,
            city: this.editOrderForm.controls['city'].value,
            postCode: this.editOrderForm.controls['postCode'].value,
            address: this.editOrderForm.controls['address'].value,
            currentStatusId: this.editOrderForm.controls['currentStatusId'].value,
            statusDeadline: this.editOrderForm.controls['statusDeadline'].value
        };

        this.dialogRef.close(this.order);
    }

}
