import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IVenueMeetingRoomViewModel, ILanguageListViewModel } from '../../models/venues.model';
import { VenuesService } from '../../services/venues.service';
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';

@Component({
  selector: 'butor-new-meeting-room',
  templateUrl: './new-meeting-room.component.html',
  styleUrls: ['./new-meeting-room.component.scss']
})
export class NewMeetingRoomComponent {

  submitted = false;

  newMeetingRoomForm: FormGroup;

  constructor(
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.newMeetingRoomForm = this.formBuilder.group({
      name: ['', Validators.required],
      location: ['', Validators.required]
    });
    if (data != null) {
      this.newMeetingRoomForm.get('name').setValue(data.data.name);
      this.newMeetingRoomForm.get('location').setValue(data.data.location);
    };
  }

  get f() { return this.newMeetingRoomForm.controls; }

  cancel() {
    this.dialogRef.close();
  }

  onSubmit() {
    this.submitted = true;

    if (this.newMeetingRoomForm.invalid) {
      return;
    }

    this.dialogRef.close(this.newMeetingRoomForm.value);
  }

}
