import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IUserTeamModel, IUserListModel } from '../../models/group.model';
import { CompanyService } from '../../services/company.service';
import { ICompanyDetailsModel } from '../../models/company.model';
import { IEmployeeModel } from '../../models/employee.model';
import { IDropDownViewModel } from '../../../../sales/appointments/models/appointments.model';

@Component({
  selector: 'butor-add-new-group',
  templateUrl: './add-new-group.component.html',
  styleUrls: ['./add-new-group.component.scss']
})
export class AddNewGroupComponent implements OnInit {

  newGroup: IUserTeamModel;
  isLoading = false;
  companyId: number;
  employees: IEmployeeModel[];
  employeesIds: number[];
  userTeamTypes: IDropDownViewModel[];

  constructor(
    public dialogRef: MatDialogRef<any>,
    private service: CompanyService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.newGroup = {
      name: '',
      users: [],
      userTeamType: undefined,
      typeName: ''
    };
  }

  typeChanged(typeObj: IDropDownViewModel) {
    this.newGroup.typeName = typeObj.name;
  }

  ngOnInit() {
    this.companyId = this.data.id;
    this.isLoading = true;
    this.service.getCompany(this.companyId).subscribe((res: ICompanyDetailsModel) => {
      this.employees = res.employees;
    },
      () => { },
      () => this.isLoading = false);
    this.service.getUserTeamTypes().subscribe(res => this.userTeamTypes = res);
  }

  cancel(): void {
    this.dialogRef.close();
  }
  addNewGroup() {
    this.dialogRef.close(this.newGroup);
  }

  employeeChange(): void {
    this.newGroup.users = [];
    this.employees.forEach(e => {
      if (this.employeesIds.includes(e.id, 0)) {
        const tempUser: IUserListModel = {
          id: e.id,
          email: e.email,
          name: e.name,
          phoneNumber: e.phone,
        };
        this.newGroup.users.push(tempUser);
      }
    });
  }

}
