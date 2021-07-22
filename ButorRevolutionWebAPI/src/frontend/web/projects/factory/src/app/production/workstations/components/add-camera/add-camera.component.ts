import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ICameraDropdownModel, ICFCProductionStateDropdownModel, IWorkstationCameraCreateModel } from '../../models/workstations.model';
import { WorkstationService } from '../../services/workstation.service';

@Component({
  selector: 'factory-add-camera',
  templateUrl: './add-camera.component.html',
  styleUrls: ['./add-camera.component.scss']
})
export class AddCameraComponent implements OnInit {

  id: number;
  cameraForm: FormGroup;
  submitted = false;
  camerasStart: ICameraDropdownModel[];
  statesStart: ICFCProductionStateDropdownModel[];
  camerasFinish: ICameraDropdownModel[];
  statesFinish: ICFCProductionStateDropdownModel[];
  constructor(
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private service: WorkstationService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    this.id = this.data.id;
    this.service.getCameras(this.id).subscribe(res => (this.camerasStart = res, this.camerasFinish = res));
    this.service.getCFCProductionStates().subscribe(res => (this.statesStart = res, this.statesFinish = res));
    this.cameraForm = this.formBuilder.group({
      state_start: [null, Validators.required],
      camera_start: [null, Validators.required],
      state_finish: [null, null],
      camera_finish: [null, null],
    });
    if (this.id) {
      this.service.getWorkstationCameras(this.id).subscribe(res => {
        this.cameraForm.controls.state_start.setValue(res.start.state);
        this.cameraForm.controls.camera_start.setValue(res.start.camera);
        if(res.finish != null){
          this.cameraForm.controls.state_finish.setValue(res.finish.state);
          this.cameraForm.controls.camera_finish.setValue(res.finish.camera);
        }
      });
    }
  }

  save(): void {
    const cameraCreateModel: IWorkstationCameraCreateModel = {
      start: {
        camera: this.cameraForm.controls.camera_start.value,
        state: this.cameraForm.controls.state_start.value
      },
      finish: {
        camera:  this.cameraForm.controls.camera_finish != undefined ? this.cameraForm.controls.camera_finish.value: null,
        state: this.cameraForm.controls.state_finish != undefined ? this.cameraForm.controls.state_finish.value : null
      }
    };
    this.service.putWorkstationCameras(this.id, cameraCreateModel).subscribe(() => this.dialogRef.close());
  }

  cancel(): void {
    this.dialogRef.close();
  }

  get f() { return this.cameraForm.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.cameraForm.invalid) {
      return;
    }

    this.save();
  }

}
