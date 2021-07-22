import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CncMachineService } from '../../services/cnc_machine.service';
import { ICncMachinesDetailsViewModel, ISupplierDropdownModel } from '../../models/cnc_machines.model';

@Component({
  selector: 'factory-new-cnc_machine',
  templateUrl: './new-cnc_machine.component.html',
  styleUrls: ['./new-cnc_machine.component.scss']
})
export class NewCncMachineComponent implements OnInit {

  id: number;
  cncMachineForm: FormGroup;
  suppliers: ISupplierDropdownModel[];
  submitted = false;
  constructor(
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private service: CncMachineService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    this.id = this.data.id;
    this.service.getSuppliers().subscribe(res => this.suppliers = res);
    this.cncMachineForm = this.formBuilder.group({
      machineName: ['', Validators.required],
      serialNumber: ['', Validators.required],
      softwareVersion: ['', Validators.required],
      code: ['', Validators.required],
      yearOfManufacture: [null, Validators.required],
      brandId: [null, Validators.required],
      sharedFolderPath: ['', Validators.required]
    });
    if (this.id) {
      this.service.getCncMachine(this.id).subscribe(res => {
        this.cncMachineForm.controls.machineName.setValue(res.machineName);
        this.cncMachineForm.controls.serialNumber.setValue(res.serialNumber);
        this.cncMachineForm.controls.softwareVersion.setValue(res.softwareVersion);
        this.cncMachineForm.controls.code.setValue(res.code);
        this.cncMachineForm.controls.yearOfManufacture.setValue(res.yearOfManufacture);
        this.cncMachineForm.controls.brandId.setValue(res.brandId);
        this.cncMachineForm.controls.sharedFolderPath.setValue(res.sharedFolderPath);
      });
    }
  }

  save(): void {
    const cncMachine: ICncMachinesDetailsViewModel = {
      id: this.id,
      machineName: this.cncMachineForm.controls.machineName.value,
      serialNumber: this.cncMachineForm.controls.serialNumber.value,
      softwareVersion: this.cncMachineForm.controls.softwareVersion.value,
      code: this.cncMachineForm.controls.code.value,
      yearOfManufacture: this.cncMachineForm.controls.yearOfManufacture.value,
      brandId: this.cncMachineForm.controls.brandId.value,
      sharedFolderPath: this.cncMachineForm.controls.sharedFolderPath.value
    };
    if (cncMachine.id) {
      this.service.updateCncMachine(cncMachine.id, cncMachine).subscribe(() => this.dialogRef.close());
    } else {
      this.service.createCncMachine(cncMachine).subscribe(() => this.dialogRef.close());
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }

  get f() { return this.cncMachineForm.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.cncMachineForm.invalid) {
      return;
    }

    this.save();
  }
}
