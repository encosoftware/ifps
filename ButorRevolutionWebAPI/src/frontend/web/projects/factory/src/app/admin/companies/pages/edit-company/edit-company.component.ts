import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ICompanyDetailsModel, IOpeningHoursModel } from '../../models/company.model';
import { Router, ActivatedRoute } from '@angular/router';
import { CompanyService } from '../../services/company.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SnackbarService } from 'butor-shared-lib';
import { CompanyTypeEnum, ClaimPolicyEnum } from '../../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'butor-edit-company',
  templateUrl: './edit-company.component.html'
})
export class EditCompanyComponent implements OnInit {

  company: ICompanyDetailsModel;
  companyId: number;
  companyEnum = CompanyTypeEnum.MyCompany;

  basicInfoForm: FormGroup;

  isLoading = false;

  claimPolicyEnum = ClaimPolicyEnum;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private service: CompanyService,
    public snackBar: SnackbarService,
    public translate: TranslateService
  ) { }

  ngOnInit() {
    this.isLoading = true;
    this.companyId = +this.route.snapshot.paramMap.get('companyId');
    this.service.getCompany(this.companyId).subscribe((res: ICompanyDetailsModel) => {
      this.company = res;
      this.basicInfoForm = this.formBuilder.group({
        name: [{ value: res.name, disabled: true }],
        companyType: res.companyTypeId,
        taxNumber: res.taxNumber,
        registerNumber: res.registerNumber,
        contactPerson: res.contactPerson.id,
        phone: [{ value: res.contactPerson.phoneNumber, disabled: true }],
        email: [{ value: res.contactPerson.email, disabled: true }],
        companyCountry: res.address.countryId,
        companyPostCode: res.address.postCode,
        companyCity: res.address.city,
        companyAddress: res.address.address,
        openingHours: this.formBuilder.array(
          res.openingHours.map((oh: IOpeningHoursModel) => {
            return this.formBuilder.group({
              openingDays: oh.dayTypeId,
              openingHour: oh.from,
              closingHour: oh.to
            });
          })
        )
      });      
    },
      () => { },
      () => this.isLoading = false);
  }

  cancel() {
    this.router.navigate(['/admin/companies/']);
  }

  saveCompany() {
    this.company.companyTypeId = this.basicInfoForm.get('companyType').value;
    this.company.taxNumber = this.basicInfoForm.get('taxNumber').value;
    this.company.registerNumber = this.basicInfoForm.get('registerNumber').value;
    this.company.contactPerson.id = this.basicInfoForm.get('contactPerson').value;
    this.company.address.countryId = this.basicInfoForm.get('companyCountry').value;
    this.company.address.postCode = this.basicInfoForm.get('companyPostCode').value;
    this.company.address.city = this.basicInfoForm.get('companyCity').value;
    this.company.address.address = this.basicInfoForm.get('companyAddress').value;
    this.company.openingHours = [];
    for (let item of this.basicInfoForm.get('openingHours').value) {
      this.company.openingHours.push({
        dayTypeId: item.openingDays,
        from: item.openingHour,
        to: item.closingHour
      });
    }
    this.service.putCompany(this.company.id, this.company).subscribe(res => {
      this.snackBar.customMessage(this.translate.instant('snackbar.saved'));
    },
      err => this.snackBar.customMessage(this.translate.instant('snackbar.error')),
      () => { }
    );
  }

}
