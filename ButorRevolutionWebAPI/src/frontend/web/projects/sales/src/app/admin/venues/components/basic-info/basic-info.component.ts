import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { IVenueBasicInfoViewModel, IVenueCountriesListViewModel, IVenueDayTypeViewModel } from '../../models/venues.model';
import { VenuesService } from '../../services/venues.service';
import { SelectDateModel } from '../../../../shared/models/selectDateModel';
import { dateSelect } from '../../../../../utils/dateSelect';

@Component({
  selector: 'butor-venue-basic-info',
  templateUrl: './basic-info.component.html',
  styleUrls: ['./basic-info.component.scss']
})
export class VenueBasicInfoComponent implements OnInit {

  @Input() basicInfo: IVenueBasicInfoViewModel;
  @Input() form: FormGroup;
  @Input() submitted: boolean;

  dateSelect: SelectDateModel[] = dateSelect();

  countries: IVenueCountriesListViewModel[];

  dayTypes: IVenueDayTypeViewModel[];

  constructor(
    private venueService: VenuesService
  ) { }

  ngOnInit() {

    this.venueService.getCountries().subscribe(res => this.countries = res);
    this.venueService.getDayTypes().subscribe(res => this.dayTypes = res);
  }

  addNewHours() {
    this.basicInfo.openingHours.push({
      dayType: this.dayTypes[0].dayType,
      from: '08:00',
      to: '16:30',
      id: this.dayTypes[0].id
    });
  }

  dayChanged(event: any, index: number) {
    this.basicInfo.openingHours[index].id = event.id;
  }

  get f() { return this.form.controls; }

  deleteOpeningHour(i: number) {
    this.basicInfo.openingHours.splice(i, 1);
  }

}
