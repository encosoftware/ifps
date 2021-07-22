import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EdgingMachineService } from '../../services/edging_machine.service';
import { IEdgingMachinesDetailsViewModel, ISupplierDropdownModel } from '../../models/edging_machines.model';

@Component({
  selector: 'factory-new-edging_machine',
  templateUrl: './new-edging_machine.component.html',
  styleUrls: ['./new-edging_machine.component.scss']
})
export class NewEdgingMachineComponent implements OnInit {

  id: number;
  edgingMachineForm: FormGroup;
  suppliers: ISupplierDropdownModel[];
  submitted = false;
  constructor(
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private service: EdgingMachineService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    this.id = this.data.id;
    this.service.getSuppliers().subscribe(res => this.suppliers = res);
    this.edgingMachineForm = this.formBuilder.group({
      machineName: ['', Validators.required],
      serialNumber: ['', Validators.required],
      softwareVersion: ['', Validators.required],
      code: ['', Validators.required],
      yearOfManufacture: [null, Validators.required],
      brandId: [null, Validators.required]
    });
    if (this.id) {
      this.service.getEdgingMachine(this.id).subscribe(res => {
        this.edgingMachineForm.controls.machineName.setValue(res.machineName);
        this.edgingMachineForm.controls.serialNumber.setValue(res.serialNumber);
        this.edgingMachineForm.controls.softwareVersion.setValue(res.softwareVersion);
        this.edgingMachineForm.controls.code.setValue(res.code);
        this.edgingMachineForm.controls.yearOfManufacture.setValue(res.yearOfManufacture);
        this.edgingMachineForm.controls.brandId.setValue(res.brandId);
      });
    }
  }

  save(): void {
    const cncMachine: IEdgingMachinesDetailsViewModel = {
      id: this.id,
      machineName: this.edgingMachineForm.controls.machineName.value,
      serialNumber: this.edgingMachineForm.controls.serialNumber.value,
      softwareVersion: this.edgingMachineForm.controls.softwareVersion.value,
      code: this.edgingMachineForm.controls.code.value,
      yearOfManufacture: this.edgingMachineForm.controls.yearOfManufacture.value,
      brandId: this.edgingMachineForm.controls.brandId.value
    };
    if (cncMachine.id) {
      this.service.updateEdgingMachine(cncMachine.id, cncMachine).subscribe(() => this.dialogRef.close());
    } else {
      this.service.createEdgingMachine(cncMachine).subscribe(() => this.dialogRef.close());
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }

  get f() { return this.edgingMachineForm.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.edgingMachineForm.invalid) {
      return;
    }

    this.save();
  }
}
