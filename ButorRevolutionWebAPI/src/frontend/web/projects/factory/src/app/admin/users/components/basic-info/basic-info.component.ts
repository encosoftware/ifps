import { Component, OnInit, Input, ViewChild, forwardRef, Optional, Inject } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  Validators,
  Validator,
  NG_VALIDATORS,
  ValidationErrors,
  AbstractControl
} from '@angular/forms';
import {
  IUserBasicInfoViewModel,
  CompanySelectModel,
  RolesSelectModel,
  CountryListSelectModel,
  ImageDetailsViewModel,
  ILanguageSelectList
} from '../../models/users.models';
import { tap, catchError, switchMap, finalize, map } from 'rxjs/operators';
import { UsersService } from '../../services/users.service';
import { of, combineLatest, throwError } from 'rxjs';
import { UploadPicService } from '../../../../shared/services/upload-pic.service';
import { LanguageTypeEnum } from '../../../../shared/clients';


@Component({
  selector: 'butor-basic-info',
  templateUrl: './basic-info.component.html',
  styleUrls: ['./basic-info.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: forwardRef(() => BasicInfoComponent),
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => BasicInfoComponent),
      multi: true
    }
  ]
})
export class BasicInfoComponent implements OnInit, ControlValueAccessor, Validator {
  countries: CountryListSelectModel[] = [];
  company: CompanySelectModel[] = [];
  roles: RolesSelectModel[] = [];
  selectedPic = null;
  isLoading = false;

  basicInfoImage: ImageDetailsViewModel =
    {
      containerName: 'ProfilePictures',
      fileName: ''
    };
  language: ILanguageSelectList[] = [];
  basicInfoForm: FormGroup;
  previewUrl: string | ArrayBuffer = '/assets/icons/photoplaceholder_.jpg';
  protected disabled = false;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UsersService,
    private picService: UploadPicService,
  ) { }

  onChange: (obj: any) => void;
  onTouched: () => void;


  writeValue(obj: IUserBasicInfoViewModel): void {
    if (obj) {
      if (obj.image) {
        this.basicInfoImage.containerName = obj.image.containerName;
        this.basicInfoImage.fileName = obj.image.fileName;
        this.picService.getThumbnail(obj.image.containerName, obj.image.fileName).pipe(
          catchError((err) => throwError(err))
        ).subscribe(
          resp => {
            this.previewUrl = resp;
          }
        );
      }
      this.basicInfoForm.patchValue(obj);
    }
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  ngOnInit() {
    this.basicInfoForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      roles: [null, [Validators.required]],
      company: null,
      phone: '',
      email: ['', [Validators.email, Validators.required]],
      address: this.formBuilder.group({
        countryId: ['', [Validators.required]],
        postCode: ['', [Validators.required]],
        city: ['', [Validators.required]],
        address: ['', [Validators.required]],
      }),
      language: [null, [Validators.required]],
    });

    this.basicInfoForm.valueChanges.pipe(
      tap((val) => {
        val.image = this.basicInfoImage;
        this.onChange(val);
      }),
    ).subscribe();
    combineLatest([
      this.userService.getCompanies(),
      this.userService.getCountries(),
      this.userService.getRoles(),
      this.userService.languageSelect(),
    ]).pipe(
      tap(([company, countries, roles, lang]:
        [CompanySelectModel[], CountryListSelectModel[], RolesSelectModel[], ILanguageSelectList[]]) => {
        this.isLoading = true;
      }),
    ).subscribe(
      ([company, countries, roles, lang]: [CompanySelectModel[], CountryListSelectModel[], RolesSelectModel[], ILanguageSelectList[]]) => {
        this.company = [...company];
        this.countries = [...countries];
        this.roles = [...roles];
        this.language = [...lang];
        this.isLoading = false;
      }
    );
  }
  validate(control: AbstractControl): ValidationErrors {
    if ((!this.basicInfoForm || this.basicInfoForm.valid)) {
      return null;
    } else {
      return {
        basicInvalid: true
      };
    }
  }
  languageSelect() {
    let names = [];
    // tslint:disable-next-line: forin
    for (let n in LanguageTypeEnum) {
      if (typeof LanguageTypeEnum[n] === 'number') {
        if (n !== 'None') { names.push({ name: n, id: LanguageTypeEnum[n] }); }
      }
    }
    return names;
  }

  onFileSelected(event) {
    this.selectedPic = event.target.files[0] as File;
    if (this.selectedPic) {
      this.picService.UploadFile(this.selectedPic, 'ProfilePictures').pipe(
        tap(res => {
          this.basicInfoImage.containerName = res.item1;
          this.basicInfoImage.fileName = res.item2;
        }),
        switchMap((pic) =>
          this.picService.getThumbnail(pic.item1, pic.item2).pipe(
            tap(res => this.previewUrl = res)
          )
        ),
        catchError((err) => of(err.message))
      ).subscribe();
    }
  }

}
