import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ICellStockListModel, INewCellViewModel } from '../../models/cells.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CellsService } from '../../services/cells.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'factory-new-cell',
  templateUrl: './new-cell.component.html',
  styleUrls: ['./new-cell.component.scss']
})
export class NewCellComponent implements OnInit {

  newCell: INewCellViewModel;
  newCellForm: FormGroup;
  submitted = false;
  title = this.translate.instant('Cells.newCell');
  stocks: ICellStockListModel[];

  constructor(
    public dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private translate: TranslateService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private cellService: CellsService
  ) { }

  ngOnInit() {
    this.cellService.getStockDropDown().subscribe(res => this.stocks = res);
    this.newCellForm = this.formBuilder.group({
      stock: [undefined, Validators.required],
      name: ['', Validators.required],
      description: ['', Validators.required]
    });
    if (this.data != null) {
      this.title = this.translate.instant('Cells.editCell');
      this.newCellForm.get('description').setValue(this.data.data.description);
      this.newCellForm.get('name').setValue(this.data.data.name);
      this.newCellForm.get('stock').setValue(this.data.data.stockId);
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }

  get f() { return this.newCellForm.controls; }

  addNewCell(): void {

    this.submitted = true;

    if (this.newCellForm.invalid) {
      return;
    }

    this.newCell = {
      stockId: this.newCellForm.get('stock').value,
      name: this.newCellForm.get('name').value,
      description: this.newCellForm.get('description').value
    };
    this.dialogRef.close(this.newCell);
  }

}
