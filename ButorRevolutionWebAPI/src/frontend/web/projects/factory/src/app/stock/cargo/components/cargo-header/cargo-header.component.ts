import { Component, OnInit, Input } from '@angular/core';
import { ICargoHeaderViewModel } from '../../models/cargo.model';

@Component({
  selector: 'factory-stock-cargo-header',
  templateUrl: './cargo-header.component.html',
  styleUrls: ['./cargo-header.component.scss']
})
export class CargoHeaderComponent implements OnInit {

  @Input() basicInfo: ICargoHeaderViewModel;
  dataSource = [];

  constructor(
  ) { }

  ngOnInit(): void {
    this.dataSource.push(this.basicInfo);
  }

}
