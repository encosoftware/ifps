import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { INewVenueViewModel, IVenueCountriesListViewModel } from '../../models/venues.model';
import { VenuesService } from '../../services/venues.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
    selector: 'butor-new-venue',
    templateUrl: './new-venue.component.html',
    styleUrls: ['./new-venue.component.scss']
})
export class NewVenueComponent implements OnInit {

    countries: IVenueCountriesListViewModel[];

    newVenueForm: FormGroup;

    venue: INewVenueViewModel;

    submitted = false;

    constructor(
        private dialogRef: MatDialogRef<any>,
        private venueService: VenuesService,
        private formBuilder: FormBuilder
    ) {
    }

    ngOnInit() {
        this.newVenueForm = this.formBuilder.group({
            name: ['', Validators.required],
            phone: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            saCountry: [undefined, Validators.required],
            saPostCode: [undefined, Validators.required],
            saCity: ['', Validators.required],
            shoppingAdress: ['', Validators.required],
        });
        this.venueService.getCountries().subscribe(res => this.countries = res);
    }

    get f() { return this.newVenueForm.controls; }

    cancel() {
        this.dialogRef.close();
    }

    addNewVenue() {
        this.venue = {
            name: this.newVenueForm.controls['name'].value,
            phone: this.newVenueForm.controls['phone'].value,
            email: this.newVenueForm.controls['email'].value,
            address: {
                address: this.newVenueForm.controls['shoppingAdress'].value,
                country: this.newVenueForm.controls['saCountry'].value,
                postCode: this.newVenueForm.controls['saPostCode'].value,
                city: this.newVenueForm.controls['saCity'].value
            }
        }
        this.dialogRef.close(this.venue);
    }

    onSubmit() {
        this.submitted = true;

        if (this.newVenueForm.invalid) {
            return;
        }

        this.addNewVenue();
    }

}
