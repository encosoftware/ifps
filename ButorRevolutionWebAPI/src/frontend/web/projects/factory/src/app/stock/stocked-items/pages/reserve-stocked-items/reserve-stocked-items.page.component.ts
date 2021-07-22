import { Component, OnInit } from '@angular/core';
import { StockedItemsService } from '../../services/stocked-items.service';
import { IStockedItemListViewModel, IOrderDropDownViewModel } from '../../models/stocked-items.model';
import { Router } from '@angular/router';
import { SnackbarService } from 'butor-shared-lib';

@Component({
  selector: 'factory-reserve-stocked-items',
  templateUrl: './reserve-stocked-items.page.component.html',
  styleUrls: ['./reserve-stocked-items.page.component.scss']
})
export class ReserveStockedItemsPageComponent implements OnInit {

  isLoading = false;
  error: string | null = null;

  dataSource: IStockedItemListViewModel[];

  orderId: string;

  orders: IOrderDropDownViewModel[];

  constructor(
    private stockedService: StockedItemsService,
    private router: Router,
    public snackBar: SnackbarService
  ) { }

  ngOnInit() {
    this.stockedService.getOrders().subscribe(res => this.orders = res);
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
    if ((this.dataSource.findIndex(x => (x.count === undefined || x.count === '')) > -1) || this.orderId === undefined) {
      return true;
    } else {
      return false;
    }
  }

  confirm(): void {
    this.stockedService.postReserveStockedItems(this.orderId, this.dataSource).subscribe(res => {
      this.snackBar.customMessage('Items reserved!');
      this.router.navigate(['/stock/stockeditems']);
    });
  }

}
