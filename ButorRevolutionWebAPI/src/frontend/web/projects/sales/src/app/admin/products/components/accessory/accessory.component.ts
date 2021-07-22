import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { IProductsAccessoriesViewModel } from '../../models/accessory.model';
import { ProductsService } from '../../services/products.service';
import { IMaterialCodeViewModel } from '../../models/front.model';

@Component({
  selector: 'butor-accessory',
  templateUrl: './accessory.component.html'
})
export class AccessoryComponent implements OnInit {

  materials: IMaterialCodeViewModel[];
  accessoryForm: FormGroup;

  submitted = false;

  accessory: IProductsAccessoriesViewModel;
  accessoryId: number;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<any>,
    public snackBar: MatSnackBar,
    private service: ProductsService
  ) { }

  ngOnInit() {
    this.accessory = {
      amount: null,
      furnitureUnitId: null,
      id: null,
      materialId: null,
      name: ''
    };

    this.accessoryForm = this.formBuilder.group({
      name: ['', Validators.required],
      amount: ['', Validators.required],
      materialId: [null, Validators.required]
    });

    this.accessory.furnitureUnitId = this.data.unitId;
    this.accessoryId = this.data.componentId ? this.data.componentId : null;
    if (this.accessoryId !== null) {
      this.service.getAccessoryComponent(this.accessoryId).subscribe(res => {
        this.accessoryForm.controls.name.setValue(res.name);
        this.accessoryForm.controls.amount.setValue(res.amount);
        this.accessoryForm.controls.materialId.setValue(res.materialId);
      });
    }
    this.service.getAccessoryCode().subscribe(res => this.materials = res);
  }

  cancel(): void {
    this.dialogRef.close();
  }

  save() {
    this.accessory.name = this.accessoryForm.controls.name.value;
    this.accessory.amount = this.accessoryForm.controls.amount.value;
    this.accessory.materialId = this.accessoryForm.controls.materialId.value;
    if (this.accessoryId === null) {
      this.service.postAccessoryComponent(this.accessory).subscribe(res => this.dialogRef.close('done'));
    } else {
      this.service.putAccessoryComponent(this.accessoryId, this.accessory).subscribe(res => this.dialogRef.close('done'));
    }
  }

  get f() { return this.accessoryForm.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.accessoryForm.invalid) {
      return;
    }

    this.save();
  }
}
