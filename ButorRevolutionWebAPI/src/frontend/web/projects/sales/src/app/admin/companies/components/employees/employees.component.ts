import { Component, OnInit, Input } from '@angular/core';
import { IEmployeeModel } from '../../models/employee.model';

@Component({
  selector: 'butor-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.scss']
})
export class EmployeesComponent implements OnInit {

  @Input() employees: IEmployeeModel[];

  constructor() { }

  ngOnInit() {
  }

}
