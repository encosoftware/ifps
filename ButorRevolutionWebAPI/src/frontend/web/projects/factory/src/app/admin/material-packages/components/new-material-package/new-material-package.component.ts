import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MaterialPackageService } from '../../services/material-packages.service';
import { IMaterialPackageDetailsModel, IMaterialDropdownModel, ISupplierDropdownModel, ICurrencyListViewModel } from '../../models/material-packages.model';

@Component({
  selector: 'factory-new-material-package',
  templateUrl: './new-material-package.component.html',
  styleUrls: ['./new-material-package.component.scss']
})
export class NewMaterialPackageComponent implements OnInit {

  id: number;
  materialPackageForm: FormGroup;
  materials: IMaterialDropdownModel[];
  suppliers: ISupplierDropdownModel[];
  currencies: ICurrencyListViewModel[];
  submitted = false;
  constructor(
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private service: MaterialPackageService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    this.service.getMaterials().subscribe(res => this.materials = res);
    this.service.getSuppliers().subscribe(res => this.suppliers = res);
    this.service.getCurrencies().subscribe(res => this.currencies = res);
    this.id = this.data.id;
    this.materialPackageForm = this.formBuilder.group({
      materialId: [null, Validators.required],
      supplierId: [null, Validators.required],
      price: [null, Validators.required],
      currencyId: [null, Validators.required],
      size: [null, Validators.required],
      code: [null, Validators.required],
      description: [null, Validators.required]
    });
    if (this.id) {
      this.service.getMaterialPackage(this.id).subscribe(res => {
        this.materialPackageForm.controls.materialId.setValue(res.materialId);
        this.materialPackageForm.controls.supplierId.setValue(res.supplierId);
        this.materialPackageForm.controls.price.setValue(res.price);
        this.materialPackageForm.controls.currencyId.setValue(res.currencyId);
        this.materialPackageForm.controls.size.setValue(res.size);
        this.materialPackageForm.controls.code.setValue(res.code);
        this.materialPackageForm.controls.description.setValue(res.description);
      });
    }
  }

  save(): void {
    const materialPackage: IMaterialPackageDetailsModel = {
      id: this.id,
      materialId: this.materialPackageForm.controls.materialId.value,
      supplierId: this.materialPackageForm.controls.supplierId.value,
      size: this.materialPackageForm.controls.size.value,
      code: this.materialPackageForm.controls.code.value,
      description: this.materialPackageForm.controls.description.value,
      price: this.materialPackageForm.controls.price.value,
      currencyId: this.materialPackageForm.controls.currencyId.value
    };
    if (materialPackage.id) {
      this.service.putMaterialPackage(materialPackage.id, materialPackage).subscribe(() => this.dialogRef.close());
    } else {
      this.service.postMaterialPackage(materialPackage).subscribe(() => this.dialogRef.close());
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }

  get f() { return this.materialPackageForm.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.materialPackageForm.invalid) {
      return;
    }

    this.save();
  }

}
