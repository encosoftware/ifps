import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import {
  ICompanyDetailsModel,
  IContactPersonModel,
  ICompanyTypeListModel,
  ICountriesListModel
} from '../../models/company.model';
import { ActivatedRoute, Router } from '@angular/router';
import { CompanyService } from '../../services/company.service';
import { debounceTime, tap, map, catchError } from 'rxjs/operators';
import { SelectDateModel } from '../../../../shared/models/selectDateModel';
import { dateSelect } from '../../../../../utils/dateSelect';
import { IVenueDayTypeViewModel } from '../../../venues/models/venues.model';
import { forkJoin, of } from 'rxjs';
import { ISelectItemModel } from '../../../../shared/models/select-items.model';

@Component({
  selector: 'butor-basic-info',
  templateUrl: './basic-info.component.html'
})
export class BasicInfoComponent implements OnInit {

  @Input() company: ICompanyDetailsModel;
  @Input() form: FormGroup;

  companyId: number;
  dayTypes: IVenueDayTypeViewModel[];
  companyTypesList: ICompanyTypeListModel[];

  daysList: ISelectItemModel[] = [];
  companyTypes: ISelectItemModel[] = [];

  countries: ICountriesListModel[];

  contactPersonList: IContactPersonModel[] = [];

  timesList: SelectDateModel[] = dateSelect();

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private service: CompanyService,
    private router: Router
  ) { }

  ngOnInit() {
    this.companyId = +this.route.snapshot.paramMap.get('companyId');
    this.searchContactPerson(this.company.contactPerson.name);
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

  get formData() { return <FormArray>this.form.get('openingHours'); }

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

  contactPersonChange(event) {
    this.form.get('phone').setValue(event.phoneNumber);
    this.form.get('email').setValue(event.email);
  }

  addNewOpeningHours() {
    this.addOpeningHourRow();
  }

  searchContactPerson(text: string) {
    if (text === '' || text === null) { return; }
    this.service.getContactPersons(text, 10).pipe(
      debounceTime(500),
      tap(() => this.contactPersonList = []),
      tap((e) => e.map((cp) => this.contactPersonList = [...this.contactPersonList, {
        id: cp.id,
        name: cp.name,
        email: cp.email,
        phoneNumber: cp.phoneNumber,
        picture: {
          containerName: cp.picture.containerName,
          fileName: cp.picture.fileName
        }
      }])),
    ).subscribe();
  }

  deleteOpeningHour(openIndex: number) {
    this.removeUserRow(openIndex);
  }

}
