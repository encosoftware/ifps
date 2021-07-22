import { Component, OnInit } from '@angular/core';
import { StockedItemsService } from '../../services/stocked-items.service';
import { IStockedItemListViewModel } from '../../models/stocked-items.model';
import { FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { Router } from '@angular/router';
import { SnackbarService } from 'butor-shared-lib';

@Component({
  selector: 'factory-eject-stocked-items',
  templateUrl: './eject-stocked-items.page.component.html',
  styleUrls: ['./eject-stocked-items.page.component.scss']
})
export class EjectStockedItemsPageComponent implements OnInit {

  isLoading = false;
  error: string | null = null;

  dataSource: IStockedItemListViewModel[];

  ejectForm: FormGroup;
  products: FormArray;

  constructor(
    private stockedService: StockedItemsService,
    private formBuilder: FormBuilder,
    private router: Router,
    public snackBar: SnackbarService
  ) { }

  ngOnInit() {
    this.ejectForm = this.formBuilder.group({
      products: this.formBuilder.array([])
    });
    this.stockedService.data.subscribe(res => {
      this.dataSource = res;
      if (res.length === 0) {
        this.router.navigate(['/stock/stockeditems']);
      }
    });
  }

  get hasError(): boolean {
    return !!this.error;
  }

  isEmptyInputField(): boolean {
    if (this.dataSource.findIndex(x => (x.count === undefined || x.count === '')) > -1) {
      return true;
    } else {
      return false;
    }
  }

  confirm(): void {
    this.stockedService.postEjectStockedItems(this.dataSource).subscribe(() => {
      this.snackBar.customMessage('Items ejected!');
      this.router.navigate(['/stock/stockeditems']);
    });
  }

}
