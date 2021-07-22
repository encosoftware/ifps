import { Component, OnInit, Input } from '@angular/core';
import { IOrderDetailsBasicInfoViewModel } from '../../../../sales/orders/models/orders';

@Component({
  selector: 'butor-order-header',
  templateUrl: './order-header.component.html',
  styleUrls: ['./order-header.component.scss']
})
export class OrderHeaderComponent implements OnInit {
  @Input() basicInfo: IOrderDetailsBasicInfoViewModel;
  dataSource = [];

  constructor(
  ) { }

  ngOnInit() {
    this.dataSource.push(this.basicInfo);
  }

}
