import { Component, OnInit, Input } from '@angular/core';
import { IContractViewModel } from '../../models/orders';
import { Router, ActivatedRoute } from '@angular/router';
import { ContractService } from '../../services/contract.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DateAdapter } from '@angular/material/core';
import { MatDialog } from '@angular/material/dialog';
import { ContractPreviewComponent } from '../contract-preview/contract-preview.component';
import { LanguageSetService } from '../../../../core/services/language-set.service';

@Component({
  selector: 'butor-order-contract-form',
  templateUrl: './contract-form.component.html',
  styleUrls: ['./contract-form.component.scss'],
})
export class OrderContractFormComponent implements OnInit {

  @Input() contract: IContractViewModel;

  createContractForm: FormGroup;
  isPreviewButtonVisible = false;
  isSaveButtonEnabled = false;
  dateMin: Date = new Date();
  orderId: string;
  lng: string;
  dateFormat: string;

  constructor(
    public dialog: MatDialog,
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private contractService: ContractService,
    private lngService: LanguageSetService,
    private dateAdapter: DateAdapter<any>
  ) { }

  ngOnInit(): void {
    this.lng = this.lngService.getLocalLanguageStorage();
    if (this.lng === 'hu-HU') {
      this.dateFormat = 'y. MM. dd.';
    } else {
      this.dateFormat = 'dd/MM/y';
    }
    this.dateAdapter.setLocale(this.lng);
    this.orderId = this.route.snapshot.paramMap.get('id');
    this.createContractForm = this.formBuilder.group({
      currencyId: this.contract.financial.currencyid,
      firstPayment: [this.contract.financial.firstPaymentPrice, Validators.required],
      firstPaymentDate: [this.contract.financial.firstPaymentDate, Validators.required],
      secondPayment: [this.contract.financial.secondPaymentPrice, Validators.required],
      secondPaymentDate: [this.contract.financial.secondPaymentDate, Validators.required],
      additional: [this.contract.additionalPoints, Validators.required],
      contractDate: [this.contract.date, Validators.required],
      contract: this.formBuilder.group({
        idNumber: [],
        total: [],
        deposit: [],
        rest: [],
        manufacturer: [],
        depositPercentage: [],
        bank: [],
        bankaccount: [],
        fromWeek: [],
        toWeek: []
      })
    });
    this.isPreviewButtonVisible = this.createContractForm.valid;

    this.createContractForm.valueChanges.subscribe(res => {
      this.isSaveButtonEnabled = this.createContractForm.valid;
    });
    
  }

  saveContract() {
    this.contractService.postContract(this.route.snapshot.paramMap.get('id'), this.createContractForm.value).subscribe(() => {
      this.isPreviewButtonVisible = this.createContractForm.valid;
    });
  }

  cancel() {
    this.router.navigate(['/sales/orders/' + this.route.snapshot.paramMap.get('id')]);
  }

  get f() { return this.createContractForm.controls; }

  openPreview() {
    const dialogRef = this.dialog.open(ContractPreviewComponent, {
      panelClass: 'preview-dialog-container',
      data: {
        id: this.orderId,
        contract: this.createContractForm.value.contract
      }
    });
  }
}
