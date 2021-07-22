import { Component, OnInit, Input } from '@angular/core';
import { InspectionService } from '../../services/inspection.service';
import { IInspectionProductListViewModel } from '../../models/inspection.model';
import { Router, ActivatedRoute } from '@angular/router';
import { SnackbarService } from 'butor-shared-lib';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'factory-inspection-details',
  templateUrl: './inspection-details.component.html',
  styleUrls: ['./inspection-details.component.scss']
})
export class InspectionDetailsComponent implements OnInit {

  @Input() products: IInspectionProductListViewModel[];
  @Input() closed: boolean;

  canCloseInspection = true;

  constructor(
    private inspectionService: InspectionService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: SnackbarService,
    private translate: TranslateService
  ) { }

  ngOnInit(): void {
    if (this.products.findIndex(x => (x.isCorrect === undefined) || (x.isCorrect === null)) > -1) {
      this.canCloseInspection = false;
    }
  }

  onCorrectChange(value: boolean, row: any) {
    let index = this.products.findIndex(res => row === res);
    if (value === true) {
      this.products[index].isCorrect = true;
    } else {
      this.products[index].isCorrect = null;
    }
    this.canCloseInspection = false;
  }

  onWrongChange(value: boolean, row: any) {
    let index = this.products.findIndex(res => row === res);
    if (value === true) {
      this.products[index].isCorrect = false;
    } else {
      this.products[index].isCorrect = null;
    }
    this.canCloseInspection = false;
  }

  cancel() {
    this.router.navigate(['/stock/inspection']);
  }

  closeCargos() {
    this.inspectionService.closeInspectionReport(+this.route.snapshot.paramMap.get('id')).subscribe(res => {
      this.snackBar.customMessage(this.translate.instant('snackbar.success'));
      this.router.navigate(['/stock/inspection']);
    });
  }

  saveCargos() {
    this.inspectionService.putInspectionReport(+this.route.snapshot.paramMap.get('id'), this.products).subscribe(res => {
      this.snackBar.customMessage(this.translate.instant('snackbar.saved'));
      this.canCloseInspection = true;
    });
  }

  isEmptyInputField(): boolean {
    if (this.products.findIndex(x => (x.isCorrect === undefined) || (x.isCorrect === false && (x.missing === undefined || x.missing === null))) > -1) {
      return true;
    } else {
      return false;
    }
  }

}
