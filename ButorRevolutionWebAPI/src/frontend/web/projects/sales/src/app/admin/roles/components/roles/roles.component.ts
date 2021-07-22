import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { IDivisionViewModel } from '../../models/roles.model';


@Component({
  selector: 'butor-role-list',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent implements OnInit {

  @Input()
  model: IDivisionViewModel[];

  @Output() selectedRoleId = new EventEmitter<number>();

  selectedRole: number;

  constructor(

  ) { }

  ngOnInit() {
  }

  selectRole(id: number) {
    this.selectedRole = id;
    this.selectedRoleId.emit(id);
  }

}
