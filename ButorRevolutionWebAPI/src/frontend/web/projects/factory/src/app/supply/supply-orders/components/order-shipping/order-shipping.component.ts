import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ICargoShippingViewModel, IDropDownViewModel } from '../../models/cargo-shipping.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SupplyService } from '../../services/supply.service';
import { forkJoin, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { SnackbarService } from 'butor-shared-lib';

@Component({
  selector: 'factory-order-shipping',
  templateUrl: './order-shipping.component.html',
  styleUrls: ['./order-shipping.component.scss']
})
export class OrderShippingComponent implements OnInit {

  @Input() shipping: ICargoShippingViewModel;
  @Input() submitted;
  @Output() isValid = new EventEmitter<boolean>();
  @Output() shippingOutput = new EventEmitter<ICargoShippingViewModel>();

  orderShippingForm: FormGroup;

  currencies: IDropDownViewModel[] = [];

  countries: IDropDownViewModel[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private supplyService: SupplyService,
    private router: Router,
    private snackBar: SnackbarService
  ) { }

  ngOnInit() {
    forkJoin([
      this.supplyService.getCountries(),
      this.supplyService.getCurrencies()
    ]).pipe(
      map(([coun, curr]) => {
        this.countries = coun;
        this.currencies = curr;
      }),
      catchError(err => {
        this.snackBar.error();
        return of(this.router.navigate(['/supply/orders']));
      })
    ).subscribe();
    this.orderShippingForm = this.formBuilder.group({
      shippingCost: ['', Validators.required],
      currency: ['', Validators.required],
      country: ['', Validators.required],
      postCode: ['', Validators.required],
      city: ['', Validators.required],
      address: ['', Validators.required],
      note: '',
      vatIsChecked: null
    });

    this.orderShippingForm.valueChanges.subscribe(() => {
      this.shipping = {
        currencyId: this.orderShippingForm.get('currency').value,
        note: this.orderShippingForm.get('note').value,
        shippingCost: this.orderShippingForm.get('shippingCost').value,
        shippingAddress: {
          address: this.orderShippingForm.get('address').value,
          city: this.orderShippingForm.get('city').value,
          countryId: this.orderShippingForm.get('country').value,
          postCode: this.orderShippingForm.get('postCode').value,
        },
        vatIsChecked: this.orderShippingForm.get('vatIsChecked').value,
      };
      this.isValid.emit(!this.orderShippingForm.invalid);
      this.shippingOutput.emit(this.shipping);
    });
  }

  toggleVatAmount(): void {
    this.shipping.vatIsChecked = !this.shipping.vatIsChecked;
  }

  get f() { return this.orderShippingForm.controls; }

}
