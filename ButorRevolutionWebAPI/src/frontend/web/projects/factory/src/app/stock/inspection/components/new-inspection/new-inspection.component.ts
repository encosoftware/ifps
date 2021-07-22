import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DateAdapter } from '@angular/material/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { INewInspectionViewModel, IStocksDropDownViewModel, IUserDropDownViewModel } from '../../models/inspection.model';
import { InspectionService } from '../../services/inspection.service';
import { LanguageSetService } from '../../../../core/services/language-set.service';

@Component({
  selector: 'factory-new-inspection',
  templateUrl: './new-inspection.component.html',
  styleUrls: ['./new-inspection.component.scss']
})
export class NewInspectionComponent implements OnInit {

  newInspection: INewInspectionViewModel;

  newInspectionForm: FormGroup;

  submitted = false;
  
  stocks: IStocksDropDownViewModel[];

  users: IUserDropDownViewModel[];

  lng: string;

  constructor(
    public dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private service: InspectionService,
    private lngService: LanguageSetService,
    private dateAdapter: DateAdapter<any>
  ) { }

  ngOnInit() {
    this.lng = this.lngService.getLocalLanguageStorage();
    this.dateAdapter.setLocale(this.lng);
    this.service.getStocks().subscribe(res => this.stocks = res);
    this.service.getUsersDropDown().subscribe(res => this.users = res);
    this.newInspectionForm = this.formBuilder.group({
      stock: [undefined, Validators.required],
      delegation: [undefined, Validators.required],
      report: [undefined, Validators.required],
      date: [undefined, Validators.required]
    });
    if (this.data != null) {
      this.newInspectionForm.get('stock').setValue(this.data.data.storageId);
      this.newInspectionForm.get('delegation').setValue(this.data.data.delegationIds);
      this.newInspectionForm.get('report').setValue(this.data.data.reportName);
      this.newInspectionForm.get('date').setValue(this.data.data.inspectedOn);
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }

  get f() { return this.newInspectionForm.controls; }

  addNewInspection(): void {

    this.submitted = true;

    if (this.newInspectionForm.invalid) {
      return;
    }

    this.newInspection = {
      inspectedOn: this.newInspectionForm.get('date').value,
      reportName: this.newInspectionForm.get('report').value,
      storageId: this.newInspectionForm.get('stock').value,
      delegationIds: this.newInspectionForm.get('delegation').value
    };

    this.dialogRef.close(this.newInspection);
  }

}
