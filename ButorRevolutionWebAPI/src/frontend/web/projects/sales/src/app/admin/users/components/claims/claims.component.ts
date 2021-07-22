import {
  Component,
  OnInit,
  OnDestroy,
  forwardRef,
  Input,
  Output,
  EventEmitter
} from '@angular/core';
import {
  FormBuilder,
  FormArray,
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  Validator,
  NG_VALIDATORS,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import {
  IModuleViewModel,
  IClaimViewModel
} from '../../models/users.models';
import { tap } from 'rxjs/operators';
import { Subscription } from 'rxjs';


@Component({
  selector: 'butor-claims',
  templateUrl: './claims.component.html',
  styleUrls: ['./claims.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: forwardRef(() => ClaimsComponent),
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => ClaimsComponent),
      multi: true
    }
  ]
})
export class ClaimsComponent implements OnInit, OnDestroy, ControlValueAccessor, Validator {
  @Input() model: IModuleViewModel[];
  claimsIds = [];
  claimsFormSubscription: Subscription;
  claimsForm: FormArray;
  isLoading = false;
  @Output() isTemplateLoad = new EventEmitter();
  onChangeControl: (obj: any) => void;
  onTouchedControl: () => void;

  // tslint:disable-next-line: variable-name
  _claimsID: number[];
  get claimsID(): number[] {
    return this._claimsID;
  }

  @Input()
  set claimsID(value: number[]) {
    this._claimsID = [];
    this._claimsID = [...value, ...this.claimsIds];
    this.writeValue(this._claimsID);
  }

  constructor(
    private formBuilder: FormBuilder,
  ) { }

  ngOnInit() {
    if (this.model) {
      this.buildForm();
    }
    if (this.claimsID) {
      this.writeValue(this.claimsID);
    }
  }
  ngOnDestroy(): void {
    this.claimsFormSubscription.unsubscribe();

  }

  private buildForm(): void {
    if (this.claimsFormSubscription) {
      this.claimsFormSubscription.unsubscribe();
      this.claimsFormSubscription = undefined;
    }
    this.claimsForm = this.formBuilder.array(
      this.model.map((m: IModuleViewModel) => {
        return this.formBuilder.group({
          name: m.name,
          description: m.description,
          division: m.division,
          claims: this.formBuilder.array(
            m.claims.map((c: IClaimViewModel) => {
              return this.formBuilder.group({
                id: c.id,
                division: m.division,
                name: c.name,
                isChecked: c.isChecked
              });
            })
          )
        });
      })
    );

    this.isTemplateLoad.emit(this.isLoadTemplateArray(this.model));
    this.claimsFormSubscription = this.claimsForm.valueChanges.pipe(
      tap((val: IModuleViewModel[]) => {
        const filterByCheck = val.map(claim => claim.claims.filter(e => e.isChecked));
        const result = filterByCheck.map((c) => c.map((d) => d.id));
        // tslint:disable-next-line: no-shadowed-variable
        this.claimsIds = result.reduce((acc, val) => acc.concat(val));
        this.onChangeControl(this.claimsIds);
      }),
      tap(val => this.model = val),
    ).subscribe();
  }

  trackByFn(index, item) {
    return index;
  }

  handleModuleClick(index: number, event: Event) {
    event.stopPropagation();
    const form = this.claimsForm.get(index.toString()).get('claims');
    const claims: IClaimViewModel[] = form.value;
    const newValues = claims.every((e: IClaimViewModel) => !e.isChecked)
      ? claims.map(c => ({ ...c, isChecked: true }))
      : claims.map(c => ({ ...c, isChecked: false }));

    form.patchValue(newValues);
  }

  writeValue(value: number[]): void {
    if (value) {
      value.map((id) => this.model = this.model.map(p => ({
        name: p.name,
        description: p.description,
        division: p.division,
        claims: p.claims.map(c => c.id === id
          ? ({ ...c, isChecked: true, division: p.division })
          : c)
      })));
      this.buildForm();
    }
  }

  registerOnChange(fn: any): void {
    this.onChangeControl = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouchedControl = fn;
  }

  validate(control: AbstractControl): ValidationErrors {
    if ((!this.claimsForm || this.claimsForm.valid)) {
      return null;
    } else {
      return {
        claimsForm: true
      };
    }
  }

  isLoadTemplateArray(event: IModuleViewModel[]): string[] {
    const filter = event.map(claim => claim.claims.filter(e => e.isChecked));
    const division = filter.map((div) => div.map((d) => d.division.toString()));
    const divisionFineal = division.reduce((acc, numb) => acc.concat(numb));
    const final = this.getUnique(divisionFineal);
    return [...final];
  }

  getUnique(arr: string[]): string[] {
    const final: string[] = [];

    arr.map((e, i) => !final.includes(e) && final.push(e));
    return final;
  }
  
}
