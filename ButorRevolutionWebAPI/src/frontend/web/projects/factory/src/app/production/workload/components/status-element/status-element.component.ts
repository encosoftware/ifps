import { Component, OnInit, Input } from '@angular/core';
import { WorkStationsPlansDetailsModel } from '../../models/workload';

@Component({
  selector: 'factory-status-element',
  templateUrl: './status-element.component.html',
  styleUrls: ['./status-element.component.scss']
})
export class StatusElementComponent implements OnInit {

  @Input() workload:WorkStationsPlansDetailsModel;
  constructor() { }

  ngOnInit() {
  }

}
