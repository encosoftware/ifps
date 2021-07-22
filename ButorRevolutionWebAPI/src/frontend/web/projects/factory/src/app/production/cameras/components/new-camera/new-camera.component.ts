import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ICameraDetailsModel } from '../../models/cameras.model';
import { CameraService } from '../../services/cameras.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'factory-new-camera',
  templateUrl: './new-camera.component.html',
  styleUrls: ['./new-camera.component.scss']
})
export class NewCameraComponent implements OnInit {

  id: number;
  cameraForm: FormGroup;
  submitted = false;
  constructor(
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private service: CameraService,
    private translate: TranslateService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    this.id = this.data.id;
    this.cameraForm = this.formBuilder.group({
      name: ['', Validators.required],
      ipAddress: [null, Validators.required],
      type: [null, Validators.required]
    });
    if (this.id) {
      this.service.getCamera(this.id).subscribe(res => {
        this.cameraForm.controls.name.setValue(res.name);
        this.cameraForm.controls.ipAddress.setValue(res.ipAddress);
        this.cameraForm.controls.type.setValue(res.type);
      });
    }
  }

  save(): void {
    const camera: ICameraDetailsModel = {
      id: this.id,
      name: this.cameraForm.controls.name.value,
      ipAddress: this.cameraForm.controls.ipAddress.value,
      type: this.cameraForm.controls.type.value
    };
    if (camera.id) {
      this.service.putCamera(camera.id, camera).subscribe(() => this.dialogRef.close());
    } else {
      this.service.postCamera(camera).subscribe(() => this.dialogRef.close());
    }
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
