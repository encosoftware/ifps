import { Component, OnInit } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { INewOrderViewModel, ICountriesListViewModel, IUsersListModel } from '../../models/orders';
import { OrdersService } from '../../services/orders.service';
import { Subject } from 'rxjs';
import { Subscription, forkJoin } from 'rxjs';
import { debounceTime, tap, map, finalize, takeUntil } from 'rxjs/operators';
import { DivisionTypeEnum } from '../../../../shared/clients';
import { addDays } from 'date-fns';
import { LanguageSetService } from '../../../../core/services/language-set.service';


@Component({
  templateUrl: './new-order.component.html',
  styleUrls: ['./new-order.component.scss']
})
export class NewOrderComponent implements OnInit {

  customerText = '';
  customerCount = 10;
  customers: IUsersListModel[];
  salesText = '';
  salesCount = 10;
  sales: IUsersListModel[];

  subscription$: Subscription;
  countries: ICountriesListViewModel[];

  submitted = false;

  newOrder: INewOrderViewModel;

  newOrderForm: FormGroup;
  isLoading = false;

  dateMin: Date = addDays(new Date(), 6);

  destroy$ = new Subject();

  constructor(
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private orderService: OrdersService,
    private lngService: LanguageSetService,
    private dateAdapter: DateAdapter<any>
  ) { }

  ngOnInit(): void {
    this.dateAdapter.setLocale(this.lngService.getLocalLanguageStorage());
    this.isLoading = true;
    this.newOrderForm = this.formBuilder.group({
      customerId: [undefined, Validators.required],
      orderName: ['', Validators.required],
      address: this.formBuilder.group({
        countryId: undefined,
        postCode: ['', Validators.required],
        city: ['', Validators.required],
        address: ['', Validators.required],
      }),
      salesId: [undefined, Validators.required],
      deadline: [null, Validators.required],
    });
    this.subscription$ = forkJoin([
      this.orderService.getCountries(),
      this.orderService.userSearch('', DivisionTypeEnum.Customer, 10),
      this.orderService.userSearch('', DivisionTypeEnum.Sales, 10)
    ]).pipe(
      map(([first, customer, sales]) => {
        this.countries = first;
        this.newOrderForm.get('address').get('countryId').setValue(first[0].id);
        this.customers = customer;
        this.sales = sales;
      }),
      finalize(() => this.isLoading = false)
    ).subscribe();

  }

  onChange(event) {
    let splittedName = event.name.split(' ');
    let orderName = 'OR_' + splittedName[0].toLowerCase().charAt(0) + '_' + splittedName[1].toLowerCase();
    this.newOrderForm.controls['orderName'].setValue(orderName);
  }

  searchCustomers(text: string) {
    this.orderService.getCustomers(text, 10).pipe(
      debounceTime(500),
      tap(res => this.customers = res),
      takeUntil(this.destroy$)
    ).subscribe();
  }

  get f() { return this.newOrderForm.controls; }

  cancel(): void {
    this.dialogRef.close();
  }

  save(): any {
    this.submitted = true;

    if (this.newOrderForm.invalid) {
      return;
    }
    this.newOrder = {
      customerId: this.newOrderForm.controls['customerId'].value,
      orderName: this.newOrderForm.controls['orderName'].value,
      salesId: this.newOrderForm.controls['salesId'].value,
      deadline: this.newOrderForm.controls['deadline'].value,
      countryId: this.newOrderForm.controls['address'].value.countryId,
      city: this.newOrderForm.controls['address'].value.city,
      postCode: this.newOrderForm.controls['address'].value.postCode,
      address: this.newOrderForm.controls['address'].value.address
    };
    this.dialogRef.close(this.newOrder);
  }

  searchCustomer(text: string) {
    this.customerText = text;
    this.fetchCustomer();
  }

  scrollCustomer() {
    if (this.customerCount === this.customers.length) {
      this.customerCount += 10;
      this.fetchCustomer();
    }
  }

  fetchCustomer() {
    this.orderService.userSearch(this.customerText, DivisionTypeEnum.Customer, this.customerCount).pipe(
      tap(() => this.customers = []),
      tap((c) => c.map((x) => this.customers = [...this.customers, {
        id: x.id,
        name: x.name
      }]))
    ).subscribe();
  }

  searchSales(text) {
    this.salesText = text;
    this.fetchSales();
  }

  scrollSales() {
    if (this.salesCount === this.sales.length) {
      this.salesCount += 10;
      this.fetchSales();
    }
  }

  fetchSales() {
    this.orderService.userSearch(this.salesText, DivisionTypeEnum.Sales, this.salesCount).pipe(
      tap(() => this.sales = []),
      tap((s) => s.map((x) => this.sales = [...this.sales, {
        id: x.id,
        name: x.name
      }]))
    ).subscribe();
  }

}
