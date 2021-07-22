import { Component, OnInit, Input } from '@angular/core';
import { WebshopOrdersListViewModel } from '../../models/orders';
import { MatDialog } from '@angular/material/dialog';
import { OrderViewComponent } from '../order-view/order-view.component';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  @Input() items: WebshopOrdersListViewModel[] = [];
  constructor(
    public dialog: MatDialog,
  ) { }

  ngOnInit() {
  }
  openView(i:number) {
    const dialogRef = this.dialog.open(OrderViewComponent, {
      width: '98rem',
      data: this.items[i]
    });

    dialogRef.afterClosed().subscribe();
  }
}
