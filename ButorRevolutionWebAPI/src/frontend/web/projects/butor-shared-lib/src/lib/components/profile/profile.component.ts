import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IDropdownViewModel } from '../../models/dropdown.model';
import { ImageDetailsViewModel } from '../../models/image.details.model';
import { tap, switchMap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IUserBasicInfo } from '../../models/user.basic.model';

@Component({
  selector: 'butor-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  basicInfoForm: FormGroup;
  previewUrl: string | ArrayBuffer = '/assets/icons/photoplaceholder_.jpg';
  countries: IDropdownViewModel[] = [];
  company: IDropdownViewModel[] = [];
  languages: IDropdownViewModel[] = [];
  selectedPic = null;
  basicInfoImage: ImageDetailsViewModel = {
    containerName: 'ProfilePictures',
    fileName: ''
  };
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    this.countries = this.data.countries;
    this.languages = this.data.languages;
    this.data.picService.getThumbnail(this.data.userInfo.containerName, this.data.userInfo.fileName).subscribe(res =>
      this.previewUrl = res
    );
    this.basicInfoForm = this.formBuilder.group({
      name: [this.data.userInfo.name, [Validators.required]],
      phone: this.data.userInfo.phone,
      email: [this.data.userInfo.email, [Validators.email, Validators.required]],
      language: [this.data.userInfo.languageId, [Validators.required]],
      countryId: [this.data.userInfo.countryId, [ Validators.required]],
      postCode: [this.data.userInfo.postCode, [Validators.required]],
      city: [this.data.userInfo.city, [Validators.required]],
      address: [this.data.userInfo.address, [Validators.required]],
    });
  }

  onFileSelected(event: any) {
    this.selectedPic = event.target.files[0] as File;
    if (this.selectedPic) {
      this.data.picService.UploadFile(this.selectedPic, 'ProfilePictures').pipe(
        tap((res: any) => {
          this.basicInfoImage.containerName = res.item1;
          this.basicInfoImage.fileName = res.item2;
        }),
        switchMap((pic: any) =>
          this.data.picService.getThumbnail(pic.item1, pic.item2).pipe(
            tap(res => this.previewUrl = res.toString())
          )
        ),
        catchError((err) => of(err.message))
      ).subscribe();
    }
  }

  get f() { return this.basicInfoForm.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.basicInfoForm.invalid) {
      return;
    }

    this.save();
  }

  newPassword() {
    this.dialogRef.close(-1);
  }

  save(): void {
    const info: IUserBasicInfo = {
      address: this.basicInfoForm.controls.address.value,
      city: this.basicInfoForm.controls.city.value,
      countryId: this.basicInfoForm.controls.countryId.value,
      postCode: this.basicInfoForm.controls.postCode.value,
      email: this.basicInfoForm.controls.email.value,
      languageId: this.basicInfoForm.controls.language.value,
      name: this.basicInfoForm.controls.name.value,
      phone: this.basicInfoForm.controls.phone.value,
      containerName: this.basicInfoImage.containerName,
      fileName: this.basicInfoImage.fileName,
    };
    this.dialogRef.close(info);
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
