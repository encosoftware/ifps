import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { OrderSchedulingService } from '../../services/order-scheduling.service';
import { IOptimisationModel, IDropdownModel } from '../../models/order-scheduling.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { catchError } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { of } from 'rxjs';
import { SnackbarService } from 'butor-shared-lib';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'factory-optimisation-modal',
  templateUrl: './optimisation-modal.component.html',
  styleUrls: ['./optimisation-modal.component.scss']
})
export class OptimisationModalComponent implements OnInit {

  optimisation: IOptimisationModel;
  optimisationForm: FormGroup;
  sortingStrategies: IDropdownModel[];
  submitted = false;
  constructor(
    private service: OrderSchedulingService,
    private dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private snackbarService: SnackbarService,
    private translate: TranslateService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    this.service.getSortingStrategies().subscribe(res => this.sortingStrategies = res);
    this.optimisation = {
      orderIds: [],
      shiftLengthHours: null,
      shiftNumber: null,
      sortingStrategyTypeId: null
    };
    this.optimisation.orderIds = this.data.orderIds;
    this.optimisationForm = this.formBuilder.group({
      shiftNumber: [null, Validators.required],
      shiftLengthHours: [null, Validators.required],
      sortingStrategyTypeId: [null, Validators.required]
    });
  }

  saveOptimisation() {
    this.optimisation.shiftNumber = this.optimisationForm.controls.shiftNumber.value;
    this.optimisation.shiftLengthHours = this.optimisationForm.controls.shiftLengthHours.value;
    this.optimisation.sortingStrategyTypeId = this.optimisationForm.controls.sortingStrategyTypeId.value;
    this.service.startOptimistaion(this.optimisation).pipe(
      catchError((err: HttpErrorResponse) => {
        if (err.status === 500) {
          this.snackbarService.customMessage(this.translate.instant('snackbar.shared'));
        }
        return of();
      })
    ).subscribe(() => {
      this.snackbarService.customMessage(this.translate.instant('snackbar.success'));
      this.dialogRef.close(1);
    })
  }

  onSubmit() {
    this.submitted = true;

    if (this.optimisationForm.invalid) {
      return;
    }

    this.saveOptimisation();
  }

  get f() { return this.optimisationForm.controls; }
}
