import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { IDivisionListViewModel } from '../../models/roles.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { RolesService } from '../../services/roles.service';

@Component({
  templateUrl: './save-as.component.html',
  styleUrls: ['./save-as.component.scss']
})
export class SaveAsComponent implements OnInit {

  divisions: IDivisionListViewModel[] = [];

  submitted = false;

  newRoleForm: FormGroup;
  constructor(
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private rolesService: RolesService
  ) { }

  ngOnInit() {
    this.newRoleForm = this.formBuilder.group({
      divisionId: [null, Validators.required],
      name: ['', Validators.required]
    });
    this.rolesService.getDivisions().subscribe(res => this.divisions = res);
  }

  get f() { return this.newRoleForm.controls; }

  cancel(): void {
    this.dialogRef.close();
  }

  save(): any {
    this.submitted = true;

    if (this.newRoleForm.invalid) {
      return;
    }

    this.dialogRef.close(this.newRoleForm.value);
  }
}
