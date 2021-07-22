import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { INewStockedItemViewModel, ICellDropDownViewModel, ICodeDropDownViewModel } from '../../models/stocked-items.model';
import { StockedItemsService } from '../../services/stocked-items.service';

@Component({
  selector: 'factory-new-stocked-item',
  templateUrl: './new-stocked-item.component.html',
  styleUrls: ['./new-stocked-item.component.scss']
})
export class NewStockedItemComponent implements OnInit {

  newStockedItemForm: FormGroup;

  submitted = false;

  stockeditem: INewStockedItemViewModel;

  cells: ICellDropDownViewModel[];

  codes: ICodeDropDownViewModel[];
  
  constructor(
    public dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private stockedService: StockedItemsService
  ) { }

  ngOnInit() {
    this.newStockedItemForm = this.formBuilder.group({
      codeId: [undefined, Validators.required],
      cellId: [undefined, Validators.required],
      amount: [undefined, Validators.required]
    });
    this.stockedService.getCells().subscribe(res => this.cells = res);
    this.stockedService.getCodes().subscribe(res => this.codes = res);
    if (this.data != null) {
      this.newStockedItemForm.get('codeId').setValue(this.data.data.codeId);
      this.newStockedItemForm.get('cellId').setValue(this.data.data.cellId);
      this.newStockedItemForm.get('amount').setValue(this.data.data.amount);
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }

  get f() { return this.newStockedItemForm.controls; }

  addNewStockedItem(): void {

    this.submitted = true;

    if (this.newStockedItemForm.invalid) {
      return;
    }

    this.stockeditem = {
      codeId: this.newStockedItemForm.get('codeId').value,
      cellId: this.newStockedItemForm.get('cellId').value,
      amount: this.newStockedItemForm.get('amount').value
    };
    this.dialogRef.close(this.stockeditem);
  }

}
