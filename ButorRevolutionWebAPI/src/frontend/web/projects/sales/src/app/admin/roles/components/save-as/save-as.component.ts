import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ICategoryShortViewModel } from '../../models/roles.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { RolesService } from '../../services/roles.service';

@Component({
  templateUrl: './save-as.component.html',
  styleUrls: ['./save-as.component.scss']
})
export class SaveAsComponent implements OnInit {

  categories: ICategoryShortViewModel[] = [];

  submitted = false;

  newRoleForm: FormGroup;
  constructor(
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private rolesService: RolesService
  ) { }

  ngOnInit() {
    this.newRoleForm = this.formBuilder.group({
      categoryId: [null, Validators.required],
      name: ['', Validators.required]
    });
    this.rolesService.getDivisionDropDown().subscribe(res => {
      this.categories = res;
      this.newRoleForm.controls.categoryId.setValue(this.categories[0].id);
    });
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
