import { Component, OnInit, Inject } from '@angular/core';
import { IDecorboardsViewModel } from '../../../models/offer.models';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'butor-new-product',
  templateUrl: './new-product.component.html',
  styleUrls: ['./new-product.component.scss']
})
export class NewProductDialogComponent implements OnInit {

  isLoading = false;
  submitted = false;
  orderedItems: any[];
  items: IDecorboardsViewModel[];
  newProductForm: FormGroup;
  title: string;
  orderId: number;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit() {
    this.orderId = this.data.id;
    this.isLoading = true;
    this.orderedItems = this.data.orderedItems;
    this.newProductForm = this.formBuilder.group({
      cabinet: [null, Validators.required],
      quantity: [null, Validators.required]
    });
    this.data.getCabinets().subscribe(res => {
      this.items = res;
      this.items.forEach((x, index) => {
        this.orderedItems.forEach(y => {
          if (x.id === y.unitId) { this.items.splice(index, 1); }
        });
      });
      this.title = this.data.title;
      this.isLoading = false;
    });
  }

  get f() { return this.newProductForm.controls; }

  addCabinet(): void {
    this.submitted = true;
    if (this.newProductForm.invalid) { return; }
    this.data.add(
      this.orderId,
      this.newProductForm.controls.quantity.value,
      this.newProductForm.controls.cabinet.value
    ).subscribe(() => this.dialogRef.close());
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
