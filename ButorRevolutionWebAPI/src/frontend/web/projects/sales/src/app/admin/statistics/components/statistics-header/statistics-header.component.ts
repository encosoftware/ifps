import { Component, OnInit, Input } from '@angular/core';
import { SalesPersonSummaryViewModel } from '../../models/statistics';

@Component({
  selector: 'butor-statistics-header',
  templateUrl: './statistics-header.component.html',
  styleUrls: ['./statistics-header.component.scss']
})
export class StatisticsHeaderComponent implements OnInit {

  @Input() salesPerson : SalesPersonSummaryViewModel;
  constructor() { }

  ngOnInit() {
  }

}
