import { Component, OnInit, Input } from '@angular/core';
import { InspectionService } from '../../services/inspection.service';
import { IInspectionHeaderViewModel } from '../../models/inspection.model';

@Component({
  selector: 'factory-inspection-header',
  templateUrl: './inspection-header.component.html',
  styleUrls: ['./inspection-header.component.scss']
})
export class InspectionHeaderComponent implements OnInit {

  @Input() basicInfo: IInspectionHeaderViewModel;
  dataSource = [];

  constructor(
    private cargoService: InspectionService
  ) { }

  ngOnInit(): void {
    this.dataSource.push(this.basicInfo);
  }

}
