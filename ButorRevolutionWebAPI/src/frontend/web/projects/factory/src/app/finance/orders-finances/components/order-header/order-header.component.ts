import { Component, OnInit, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { IOrderDetailsBasicInfoViewModel } from '../../models/customerOrders';

@Component({
  selector: 'factory-order-header',
  templateUrl: './order-header.component.html',
  styleUrls: ['./order-header.component.scss']
})
export class OrderHeaderComponent implements OnInit {

  @Input() basicInfo: IOrderDetailsBasicInfoViewModel[];
  dataSource = [];

  constructor(
    public dialog: MatDialog,
  ) { }

  ngOnInit(): void {
    this.dataSource.push(...this.basicInfo);
  }



}
