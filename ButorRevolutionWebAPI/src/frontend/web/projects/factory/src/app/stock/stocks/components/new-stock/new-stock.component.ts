import { Component, OnInit, Inject } from '@angular/core';
import { INewStorageViewModel, ICountryListModel } from '../../models/stocks.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { StoragesService } from '../../services/stocks.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'factory-new-stock',
  templateUrl: './new-stock.component.html',
  styleUrls: ['./new-stock.component.scss']
})
export class NewStorageComponent implements OnInit {

  newStock: INewStorageViewModel;
  newStockForm: FormGroup;
  submitted = false;
  countries: ICountryListModel[];
  title = this.translate.instant('Storages.newStorage');

  constructor(
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private storageService: StoragesService,
    private translate: TranslateService,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) { }

  ngOnInit() {
    this.newStockForm = this.formBuilder.group({
      name: ['', Validators.required],
      country: [null, Validators.required],
      postCode: [null, Validators.required],
      city: ['', Validators.required],
      address: ['', Validators.required]
    });
    if (this.data !== null) {
      this.title = this.translate.instant('Storages.editStorage');
      this.newStockForm.get('name').setValue(this.data.data.name);
      this.newStockForm.get('country').setValue(this.data.data.address.country);
      this.newStockForm.get('postCode').setValue(this.data.data.address.postCode);
      this.newStockForm.get('city').setValue(this.data.data.address.city);
      this.newStockForm.get('address').setValue(this.data.data.address.address);
    }
    this.storageService.getCountries().subscribe(res => this.countries = res);
  }

  get f() { return this.newStockForm.controls; }

  cancel(): void {
    this.dialogRef.close();
  }

  addNewStock(): void {
    this.submitted = true;

    if (this.newStockForm.invalid) {
      return;
    }

    this.newStock = {
      address: {
        country: this.newStockForm.get('country').value,
        postCode: this.newStockForm.get('postCode').value,
        city: this.newStockForm.get('city').value,
        address: this.newStockForm.get('address').value,
      },
      name: this.newStockForm.get('name').value
    };

    this.dialogRef.close(this.newStock);
  }

}
