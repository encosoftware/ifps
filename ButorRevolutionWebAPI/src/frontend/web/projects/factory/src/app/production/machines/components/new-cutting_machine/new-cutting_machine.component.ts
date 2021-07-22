import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CuttingMachineService } from '../../services/cutting_machine.service';
import { ICuttingMachinesDetailsViewModel, ISupplierDropdownModel } from '../../models/cutting_machines.model';

@Component({
  selector: 'factory-new-cutting_machine',
  templateUrl: './new-cutting_machine.component.html',
  styleUrls: ['./new-cutting_machine.component.scss']
})
export class NewCuttingMachineComponent implements OnInit {

  id: number;
  cuttingMachineForm: FormGroup;
  suppliers: ISupplierDropdownModel[];
  submitted = false;
  constructor(
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private service: CuttingMachineService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    this.id = this.data.id;
    this.service.getSuppliers().subscribe(res => this.suppliers = res);
    this.cuttingMachineForm = this.formBuilder.group({
      machineName: ['', Validators.required],
      serialNumber: ['', Validators.required],
      softwareVersion: ['', Validators.required],
      code: ['', Validators.required],
      yearOfManufacture: [null, Validators.required],
      brandId: [null, Validators.required]
    });
    if (this.id) {
      this.service.getCuttingMachine(this.id).subscribe(res => {
        this.cuttingMachineForm.controls.machineName.setValue(res.machineName);
        this.cuttingMachineForm.controls.serialNumber.setValue(res.serialNumber);
        this.cuttingMachineForm.controls.softwareVersion.setValue(res.softwareVersion);
        this.cuttingMachineForm.controls.code.setValue(res.code);
        this.cuttingMachineForm.controls.yearOfManufacture.setValue(res.yearOfManufacture);
        this.cuttingMachineForm.controls.brandId.setValue(res.brandId);
      });
    }
  }

  save(): void {
    const cncMachine: ICuttingMachinesDetailsViewModel = {
      id: this.id,
      machineName: this.cuttingMachineForm.controls.machineName.value,
      serialNumber: this.cuttingMachineForm.controls.serialNumber.value,
      softwareVersion: this.cuttingMachineForm.controls.softwareVersion.value,
      code: this.cuttingMachineForm.controls.code.value,
      yearOfManufacture: this.cuttingMachineForm.controls.yearOfManufacture.value,
      brandId: this.cuttingMachineForm.controls.brandId.value
    };
    if (cncMachine.id) {
      this.service.updateCuttingMachine(cncMachine.id, cncMachine).subscribe(() => this.dialogRef.close());
    } else {
      this.service.createCuttingMachine(cncMachine).subscribe(() => this.dialogRef.close());
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }

  get f() { return this.cuttingMachineForm.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.cuttingMachineForm.invalid) {
      return;
    }

    this.save();
  }
}
