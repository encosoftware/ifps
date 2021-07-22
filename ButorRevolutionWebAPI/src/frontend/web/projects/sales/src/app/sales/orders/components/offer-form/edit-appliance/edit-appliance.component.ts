import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { OfferService } from '../../../services/offer.service';

@Component({
  selector: 'butor-edit-appliance',
  templateUrl: './edit-appliance.component.html',
  styleUrls: ['./edit-appliance.component.scss']
})
export class EditApplianceComponent implements OnInit {

  isLoading = false;
  submitted = false;
  name: string;
  applianceForm: FormGroup;
  orderId: string;
  applianceMaterialId: number;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private service: OfferService,
  ) { }

  ngOnInit() {
    this.isLoading = true;
    this.applianceForm = this.formBuilder.group({
      quantity: [null, Validators.required]
    });
    this.applianceForm.controls.quantity.setValue(this.data.quantity);
    this.isLoading = false;
    this.name = this.data.name;
    this.orderId = this.data.orderId;
    this.applianceMaterialId = this.data.applianceMaterialId;
  }

  get f() { return this.applianceForm.controls; }

  cancel(): void {
    this.dialogRef.close();
  }

  save(): void {
    this.submitted = true;
    if (this.applianceForm.invalid) { return; }
    this.isLoading = true;
    this.service.putAppliances(this.orderId, this.applianceMaterialId, this.applianceForm.controls.quantity.value).subscribe(() => {
      this.isLoading = false;
      this.dialogRef.close();
    });
  }

}
