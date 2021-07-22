import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import {
  ICompanyDetailsModel,
  ICompanyTypeListModel,
  ICountriesListModel,
  IDayTypesListModel
} from '../../models/company.model';
import { ActivatedRoute, Router } from '@angular/router';
import { CompanyService } from '../../services/company.service';
import { debounceTime, tap, catchError, map } from 'rxjs/operators';
import { ISelectItemModel } from '../../../../shared/models/selet-model';
import { of, forkJoin } from 'rxjs';
import { dateSelect } from '../../../../../utils/dateSelect';

@Component({
  selector: 'butor-basic-info',
  templateUrl: './basic-info.component.html'
})
export class BasicInfoComponent implements OnInit {

  @Input() company: ICompanyDetailsModel;
  @Input() form: FormGroup;

  companyId: number;
  dayTypes: IDayTypesListModel[];
  companyTypesList: ICompanyTypeListModel[];

  daysList: ISelectItemModel[] = [];
  companyTypes: ISelectItemModel[] = [];

  countries: ICountriesListModel[];

  timesList = dateSelect();

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private service: CompanyService,
    private router: Router
  ) { }

  ngOnInit() {
    this.companyId = +this.route.snapshot.paramMap.get('companyId');
    forkJoin([
      this.service.getCountries(),
      this.service.getDayTypes(),
      this.service.getCompanyTypes()
    ]).pipe(
      map(([coun, days, comp]) => {
        this.countries = coun;
        this.dayTypes = days;
        this.companyTypesList = comp;
      }),
      catchError( err => of(this.router.navigate(['/admin/companies/'])))
    ).subscribe();
  }

  get openingForm() { return this.form.get('openingHours') as FormArray; }

  initOpeningHourRow(): FormGroup {
    return this.formBuilder.group({
      openingDays: this.dayTypes[0].id,
      openingHour: '08:00',
      closingHour: '16:30',
    });
  }

  addOpeningHourRow(): void {
    const openingHours = this.form.controls.openingHours as FormArray;
    openingHours.push(this.initOpeningHourRow());
  }

  removeUserRow(rowIndex: number): void {
    const openingHours = this.form.controls.openingHours as FormArray;
    openingHours.removeAt(rowIndex);
  }

  addNewOpeningHours() {
    this.addOpeningHourRow();
  }

  deleteOpeningHour(openIndex: number) {
    this.removeUserRow(openIndex);
  }

}
