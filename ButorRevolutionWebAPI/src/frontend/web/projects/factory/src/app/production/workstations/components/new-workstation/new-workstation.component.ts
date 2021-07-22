import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IWorkstationDetailsModel, IMachineDropdownModel, IWorkstationDropdownModel } from '../../models/workstations.model';
import { WorkstationService } from '../../services/workstation.service';

@Component({
  selector: 'factory-new-workstation',
  templateUrl: './new-workstation.component.html',
  styleUrls: ['./new-workstation.component.scss']
})
export class NewWorkstationComponent implements OnInit {

  id: number;
  workstationForm: FormGroup;
  submitted = false;
  types: IWorkstationDropdownModel[];
  machines: IMachineDropdownModel[];
  constructor(
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private service: WorkstationService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    this.id = this.data.id;
    this.service.getMachines().subscribe(res => (this.machines = res));
    this.service.getWorkstationTypes().subscribe(res => (this.types = res));
    this.workstationForm = this.formBuilder.group({
      name: ['', Validators.required],
      optimalCrew: [null, Validators.required],
      machine: [null, Validators.required],
      type: [null, Validators.required]
    });
    if (this.id) {
      this.service.getWorkstation(this.id).subscribe(res => {
        this.workstationForm.controls.name.setValue(res.name);
        this.workstationForm.controls.optimalCrew.setValue(res.optimalCrew);
        this.workstationForm.controls.machine.setValue(res.machine);
        this.workstationForm.controls.type.setValue(res.type);
      });
    }
  }

  save(): void {
    const workstation: IWorkstationDetailsModel = {
      id: this.id,
      name: this.workstationForm.controls.name.value,
      optimalCrew: this.workstationForm.controls.optimalCrew.value,
      machine: this.workstationForm.controls.machine.value,
      type: this.workstationForm.controls.type.value,
      isActive: true
    };
    if (workstation.id) {
      this.service.putWorkstation(workstation.id, workstation).subscribe(() => this.dialogRef.close());
    } else {
      this.service.postWorkstation(workstation).subscribe(() => this.dialogRef.close());
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }

  get f() { return this.workstationForm.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.workstationForm.invalid) {
      return;
    }

    this.save();
  }

}
