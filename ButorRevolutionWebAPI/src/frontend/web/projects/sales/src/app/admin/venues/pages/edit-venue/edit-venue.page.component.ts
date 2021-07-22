import { Component, OnInit } from '@angular/core';
import { IVenueViewModel } from '../../models/venues.model';
import { Router, ActivatedRoute } from '@angular/router';
import { VenuesService } from '../../services/venues.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SnackbarService } from 'butor-shared-lib';
import { TranslateService } from '@ngx-translate/core';

@Component({
  templateUrl: './edit-venue.page.component.html',
  styleUrls: ['./edit-venue.page.component.scss']
})
export class EditVenuePageComponent implements OnInit {

  isLoading = false;
  venue: IVenueViewModel;
  id: number;

  venueBasicInfoForm: FormGroup;

  submitted = false;

  constructor(
    private router: Router,
    private service: VenuesService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    public snackBar: SnackbarService,
    public translate: TranslateService
  ) {
  }

  ngOnInit() {
    this.id = +this.route.snapshot.paramMap.get('id');
    this.isLoading = true;
    this.service.getVenue(this.id).subscribe(res => {
      this.venue = res;
      this.venueBasicInfoForm = this.formBuilder.group({
        name: [res.basicInfo.name, Validators.required],
        phone: [res.basicInfo.phone, Validators.required],
        email: [res.basicInfo.email, [Validators.required, Validators.email]],
        saCountry: [res.basicInfo.address.country, Validators.required],
        saPostCode: [res.basicInfo.address.postCode, Validators.required],
        saCity: [res.basicInfo.address.city, Validators.required],
        shoppingAddress: [res.basicInfo.address.address, Validators.required]
      });
    },
      () => { },
      () => this.isLoading = false);
  }

  cancel() {
    this.router.navigate(['/admin/venues/']);
  }

  save() {
    this.submitted = true;

    if (this.venueBasicInfoForm.invalid) {
      return;
    }
    this.venue.basicInfo.name = this.venueBasicInfoForm.controls.name.value;
    this.venue.basicInfo.phone = this.venueBasicInfoForm.controls.phone.value;
    this.venue.basicInfo.email = this.venueBasicInfoForm.controls.email.value;
    this.venue.basicInfo.address.country = this.venueBasicInfoForm.controls.saCountry.value;
    this.venue.basicInfo.address.postCode = this.venueBasicInfoForm.controls.saPostCode.value;
    this.venue.basicInfo.address.city = this.venueBasicInfoForm.controls.saCity.value;
    this.venue.basicInfo.address.address = this.venueBasicInfoForm.controls.shoppingAddress.value;
    this.isLoading = true;
    this.service.putVenue(this.id, this.venue).subscribe(() => {
      this.snackBar.customMessage(this.translate.instant('snackbar.saved'));
    },
      () => {
        this.snackBar.customMessage(this.translate.instant('snackbar.error'));
      },
      () => this.isLoading = false);
  }

  delete() {
    this.isLoading = true;
    this.service.deleteVenue(this.id).subscribe(() => {
      this.snackBar.customMessage(this.translate.instant('snackbar.deleted'));
      this.router.navigate(['/admin/venues/']);
    },
      () => { },
      () => this.isLoading = false);
  }

  deactivate() {
    this.isLoading = true;
    this.service.deactivateVenue(this.id).subscribe(() => {
      this.router.navigate(['/admin/venues/']);
    },
      () => { },
      () => this.isLoading = false);
  }

}
