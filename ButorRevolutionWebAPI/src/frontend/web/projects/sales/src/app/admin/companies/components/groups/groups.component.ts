import { Component, OnInit, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddNewGroupComponent } from '../add-new-group/add-new-group.component';
import { IUserTeamModel } from '../../models/group.model';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'butor-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.scss']
})
export class GroupsComponent implements OnInit {

  @Input() userTeams: IUserTeamModel[];
  userTeamGroup: IUserTeamModel[];
  companyId: number;

  constructor(
    public dialog: MatDialog,
    private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    this.companyId = +this.route.snapshot.paramMap.get('companyId');
    this.userTeamGroup = this.userTeams;
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(AddNewGroupComponent, {
      width: '48rem',
      data: {
        id: this.companyId
      }
    });

    dialogRef.afterClosed().subscribe((result: IUserTeamModel) => {
      if (result !== undefined) {
        this.userTeams.push(result);
        this.userTeamGroup = [...this.userTeams];
      }
    });
  }

  deleteGroup(group) {
    const index = this.userTeams.indexOf(group);
    if (index > -1) {
      this.userTeams.splice(index, 1);
      this.userTeamGroup.splice(index, 1);
      this.userTeamGroup = [...this.userTeamGroup];
    }
  }
}
