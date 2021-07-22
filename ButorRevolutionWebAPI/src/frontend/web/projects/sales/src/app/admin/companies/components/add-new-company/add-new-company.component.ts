import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ICompanyCreateModel, ICountriesListModel, ICompanyTypeListModel } from '../../models/company.model';
import { CompanyService } from '../../services/company.service';
import { SnackbarService } from 'butor-shared-lib';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'butor-add-new-company',
  templateUrl: './add-new-company.component.html'
})
export class AddNewCompanyComponent implements OnInit {

  company: ICompanyCreateModel;

  companyTypesList: ICompanyTypeListModel[] = [];
  selectedCompanyType: number;

  countries: ICountriesListModel[];

  addNewCompanyForm: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<any>,
    private router: Router,
    private formBuilder: FormBuilder,
    private service: CompanyService,
    private snackbar: SnackbarService,
    private translate: TranslateService
  ) {
  }

  ngOnInit() {
    this.service.getCompanyTypes().subscribe(res => {
      this.companyTypesList = res;
    });
    this.service.getCountries().subscribe(res => this.countries = res);
    this.addNewCompanyForm = this.formBuilder.group({
      name: ['', Validators.required],
      companyType: [null, Validators.required],
      taxnumber: [null, Validators.required],
      registernumber: [null, Validators.required],
      companyCountry: [null, Validators.required],
      companyPostCode: [null, Validators.required],
      companyCity: ['', Validators.required],
      companyAddress: ['', Validators.required]
    });
  }

  cancel(): void {
    this.dialogRef.close();
  }

  addNewCompany() {
    this.company = {
      name: this.addNewCompanyForm.get('name').value,
      registerNumber: this.addNewCompanyForm.get('registernumber').value,
      taxNumber: this.addNewCompanyForm.get('taxnumber').value,
      address: {
        countryId: this.addNewCompanyForm.get('companyCountry').value,
        postCode: this.addNewCompanyForm.get('companyPostCode').value,
        city: this.addNewCompanyForm.get('companyCity').value,
        address: this.addNewCompanyForm.get('companyAddress').value
      },
      companyTypeId: this.addNewCompanyForm.get('companyType').value
    };
    this.service.postCompany(this.company).subscribe(id => {
      this.dialogRef.close();
      this.router.navigate(['/admin/companies/edit/' + id]);
      this.snackbar.customMessage(this.translate.instant('snackbar.success'));
    },
    (err) => {
      this.dialogRef.close();
      this.snackbar.customMessage(this.translate.instant('snackbar.error'));
    });
  }

}
