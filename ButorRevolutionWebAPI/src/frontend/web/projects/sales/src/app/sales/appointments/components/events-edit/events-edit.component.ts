import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators, FormControl, FormArray } from '@angular/forms';
import { format } from 'date-fns';
import { SelectDateModel } from '../../../../shared/models/selectDateModel';
import { shortDateSelect } from '../../../../../utils/dateSelect';
import { AppointmentsService } from '../../services/appointment.service';
import { debounceTime, tap, takeUntil, map, switchMap } from 'rxjs/operators';
import { IPersonViewModel, IDropDownViewModel, IDropDownStringViewModel } from '../../models/appointments.model';
import { Subject } from 'rxjs';
import { NgSelectComponent } from 'butor-shared-lib';
import { LanguageSetService } from '../../../../core/services/language-set.service';

@Component({
  selector: 'butor-events-edit',
  templateUrl: './events-edit.component.html',
  styleUrls: ['./events-edit.component.scss']
})
export class EventsEditComponent implements OnInit {

  editAppointmentForm: FormGroup;
  submitted = false;
  dateSelect: SelectDateModel[] = shortDateSelect();
  toDateSelect: SelectDateModel[] = shortDateSelect();
  customers: IDropDownViewModel[] = [];
  participants: IPersonViewModel[] = [];
  venues: IDropDownViewModel[] = [];
  ordersList: IDropDownStringViewModel[] = [];
  meetingRooms: IDropDownViewModel[] = [];
  appointmentTypes: IDropDownViewModel[] = [];
  countries: IDropDownViewModel[] = [];
  destroy$ = new Subject();
  customerName: string;
  isDatesCorrect = true;
  @ViewChild('orderIdDom', {static: true}) orderIdSelect: NgSelectComponent;
  @ViewChild('customerIdDom', {static: true}) customerIdSelect: NgSelectComponent;
  isTechnicalAccount = false;

  constructor(
    public dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public appointmentService: AppointmentsService,
    private lngService: LanguageSetService,
    private dateAdapter: DateAdapter<any>
  ) { }

  ngOnInit() {
    this.dateAdapter.setLocale(this.lngService.getLocalLanguageStorage());
    this.editAppointmentForm = this.formBuilder.group({
      subject: ['', Validators.required],
      categoryId: [undefined, Validators.required],
      customerId: [undefined, Validators.required],
      orderId: '',
      partnerId: [undefined, Validators.required],
      date: [new Date(this.data.date), Validators.required],
      from: [undefined, Validators.required],
      to: [undefined, Validators.required],
      venueId: [undefined, Validators.required],
      meetingRoomId: [undefined, Validators.required],
      notes: '',
      isNewAddress: false,
      address: this.formBuilder.group({
        countryId: [undefined, Validators.required],
        postCode: [undefined, Validators.required],
        city: ['', Validators.required],
        address: ['', Validators.required]
      })
    });

    if (this.data.fromDate) {
      this.editAppointmentForm.get('from').setValue(format(this.data.fromDate, 'HH:mm'));
      this.editAppointmentForm.get('from').setValue(format(this.data.toDate, 'HH:mm'));
    }

    this.appointmentService.getOrdersList().pipe(
      map(res => this.ordersList = res)
    ).subscribe();

    this.editAppointmentForm.get('address').disable();
    this.editAppointmentForm.get('meetingRoomId').disable();

    this.editAppointmentForm.get('isNewAddress').valueChanges.pipe(tap(res => {
      if (!this.isTechnicalAccount) {
        if (res === true) {
          this.editAppointmentForm.get('address').enable();
          this.editAppointmentForm.get('meetingRoomId').disable();
          this.editAppointmentForm.get('meetingRoomId').setValue(0);
          this.editAppointmentForm.get('venueId').disable();
          this.editAppointmentForm.get('venueId').setValue(0);
        } else {
          this.editAppointmentForm.get('address').get('countryId').setValue(undefined);
          this.editAppointmentForm.get('address').get('postCode').setValue(undefined);
          this.editAppointmentForm.get('address').get('city').setValue('');
          this.editAppointmentForm.get('address').get('address').setValue('');
          this.editAppointmentForm.get('address').disable();
          this.editAppointmentForm.get('venueId').enable();
        }
      }
    })).subscribe();

    this.editAppointmentForm.get('orderId').valueChanges.pipe(
      switchMap((res) => this.appointmentService.getCustomerNameByOrderId(res).pipe(
        map(list => this.customers = list),
        tap(x => {
          this.editAppointmentForm.get('customerId').setValue(this.customers[0].id);
        })
      )),
    ).subscribe();

    this.countries = this.data.countries;
    this.appointmentTypes = this.data.types;
    this.participants = this.data.sales;
    this.venues = this.data.venues;

    if (this.data.data) {
      this.customerIdSelect.setDisabledState(true);
      this.orderIdSelect.setDisabledState(true);

      this.editAppointmentForm.get('orderId').setValue(this.data.data.orderId);
      this.editAppointmentForm.patchValue(this.data.data);
      if (this.data.data.venueId > 0) {
        this.appointmentService.getMeetingRooms(this.data.data.venueId).subscribe(res => {
          this.meetingRooms = res;
          this.editAppointmentForm.get('meetingRoomId').enable();
        });
      }
      this.toDateSelect = this.toDateSelect.filter(y => y.name > this.editAppointmentForm.controls.from.value);
    } else {

      this.toDateSelect = this.dateSelect;
      let o = this.dateSelect.find(x => x.name === this.editAppointmentForm.controls.from.value);
      this.toDateSelect = this.toDateSelect.filter(y => y.date > o.date);
      this.editAppointmentForm.controls.to.setValue(this.toDateSelect[0].name);
    }

    this.editAppointmentForm.controls.from.valueChanges.subscribe(res => {
      this.toDateSelect = this.dateSelect;
      let dateObject = this.dateSelect.find(x => x.name === res);
      this.toDateSelect = this.toDateSelect.filter(y => y.date > dateObject.date);
      this.editAppointmentForm.controls.to.setValue(this.toDateSelect[0].name);
    });

  }

  cancel() {
    this.dialogRef.close();
  }

  searchCustomers(text: string) {
    this.appointmentService.getContactPersons(text, 10).pipe(
      debounceTime(500),
      tap(res => this.customers = res),
      takeUntil(this.destroy$)
    ).subscribe();
  }

  venueChanged(id: number) {
    this.appointmentService.getMeetingRooms(id).subscribe(res => {
      this.meetingRooms = res;
      this.editAppointmentForm.get('meetingRoomId').enable();
      this.editAppointmentForm.controls.meetingRoomId.setValue(res[0].id);
    });
  }

  salesChange(person: IPersonViewModel) {
    this.isTechnicalAccount = person.isTechnicalAccount;
    if (person.isTechnicalAccount) {
      this.editAppointmentForm.get('isNewAddress').setValue(true);
      this.editAppointmentForm.get('isNewAddress').disable();
      this.editAppointmentForm.get('address').enable();
      this.editAppointmentForm.get('meetingRoomId').disable();
      this.editAppointmentForm.get('meetingRoomId').setValue(0);
      this.editAppointmentForm.get('venueId').disable();
      this.editAppointmentForm.get('venueId').setValue(0);
    } else {
      this.editAppointmentForm.get('isNewAddress').enable();
    }
  }

  get f() { return this.editAppointmentForm.controls; }
  get g() { return (this.editAppointmentForm.controls.address as FormGroup).controls; }

  save() {
    this.submitted = true;

    let fromArray = this.editAppointmentForm.get('from').value.split(':');
    let toArray = this.editAppointmentForm.get('to').value.split(':');
    let from = new Date(Date.UTC(
      this.editAppointmentForm.get('date').value.getFullYear(),
      this.editAppointmentForm.get('date').value.getMonth(),
      this.editAppointmentForm.get('date').value.getDate(),
      +fromArray[0],
      +fromArray[1])
    );
    let to = new Date(Date.UTC(
      this.editAppointmentForm.get('date').value.getFullYear(),
      this.editAppointmentForm.get('date').value.getMonth(),
      this.editAppointmentForm.get('date').value.getDate(),
      +toArray[0],
      +toArray[1])
    );

    if (from >= to) {
      this.isDatesCorrect = false;
    } else {
      this.isDatesCorrect = true;
    }

    if (this.editAppointmentForm.invalid || !this.isDatesCorrect) {
      return;
    }
    this.dialogRef.close(this.editAppointmentForm.value);
  }

}
