import { Component, forwardRef, Input, OnInit } from '@angular/core';
import {
  ControlValueAccessor, NG_VALUE_ACCESSOR
} from '@angular/forms';
import { tap, debounceTime, distinctUntilChanged, take, switchMap } from 'rxjs/operators';
import {
  IUserWorkingInfoViewModel, IUserWorkingHour, IUserWorkingHoursModel,
  UsersOfficesModel, UserSalesModel, DayTypeListModel
} from '../../models/users.models';
import { dateSelect } from '../../../../../utils/dateSelect';
import { SelectDateModel } from '../../../../shared/models/selectDateModel';
import { UsersService } from '../../services/users.service';
import { format } from 'date-fns';
import { DayTypeEnum } from '../../../../shared/clients';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../../../core/store/selectors/core.selector';

@Component({
  selector: 'butor-working-info',
  templateUrl: './working-info.component.html',
  styleUrls: ['./working-info.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: forwardRef(() => WorkingInfoComponent),
    }
  ]
})
export class WorkingInfoComponent implements ControlValueAccessor, OnInit {

  controlOnChange: (obj: IUserWorkingInfoViewModel) => void;

  model: IUserWorkingInfoViewModel = {
    usersOffices: [],
    supervisors: null,
    supervisees: [],
    groups: [],
    discount: { from: null, to: null },
    workingHours: [],
  };

  dateSelect: SelectDateModel[] = dateSelect();
  companyId: number;

  @Input() workingHours: DayTypeListModel[];

  usersOfficesSelect: UsersOfficesModel[] = [];
  supervisorsSelect: UserSalesModel[] = [];

  constructor(
    private userService: UsersService,
    private store: Store<any>,

  ) { }

  ngOnInit(): void {
    this.store.pipe(
      select(coreLoginT),
      take(1),
      tap((resp) => {
        this.companyId = +resp.CompanyId;
      })
    ).subscribe();
    this.userService.searchUsersOffices('', this.companyId).pipe(
      tap((_) => {
        this.usersOfficesSelect = [];
      }),
      tap((e) => {
        if (e) {
          this.usersOfficesSelect = e.map((c) =>  ({ id: c.id, name: c.name }))
        }
      }
      ),
      switchMap(res => 
        this.userService.searchSalesPerson('').pipe(
          tap((_) => this.supervisorsSelect = []),
          tap((e) => this.supervisorsSelect = e.map((c) =>  ({ id: c.id, name: c.name }))),
        ))
    ).subscribe();
  }

  removeHours(day: number, index: number) {
    this.model.workingHours[day].workingHour.splice(index, 1);
  }

  addNewHours(index: number) {
    this.model.workingHours[index].workingHour.push({
      dayTypeId: index + 1,
      id: null,
      from: '08:00',
      to: '16:30',
    });
  }

  writeValue(obj: IUserWorkingInfoViewModel): void {
    if (obj) {
      this.usersOfficesSelect = obj.usersOfficesList ? [...obj.usersOfficesList] : [];
      this.supervisorsSelect = obj.supervisorsList ? [obj.supervisorsList] : [];
      this.model.usersOffices = obj.usersOffices.length ? [...obj.usersOffices] : [];
      this.model.supervisors = obj.supervisors;
      this.model.supervisees = obj.supervisees;
      this.model.groups = obj.groups;
      this.model.discount = obj.discount;
      this.model.workingHours = this.workingHours.map((wh): IUserWorkingHoursModel => {
        let hours = obj.workingHours.filter(x => {
          return x.dayTypeId === wh.id;
        });
        return {
          day: wh.translation,
          dayTypeId: wh.id,
          isChecked: hours.length ? true : false,
          workingHour: hours.length ? hours.map((h: IUserWorkingHour): IUserWorkingHour =>
            ({
              dayTypeId: h.dayTypeId,
              from: (h.from instanceof Date) ? format(h.from, 'HH:mm') : h.from,
              to: (h.to instanceof Date) ? format(h.to, 'HH:mm') : h.to,
              id: h.id
            })
          ) : [({
            dayTypeId: +DayTypeEnum[wh.dayType],
            from: '08:00',
            to: '16:30',
            id: null
          })]

        };
      });
      this.controlOnChange(this.model);
    } else {
      this.model.workingHours = this.workingHours.map((wh): IUserWorkingHoursModel => {
        return {
          day: wh.translation,
          dayTypeId: wh.id,
          isChecked: false,
          workingHour: [({
            dayTypeId: +DayTypeEnum[wh.dayType],
            from: '08:00',
            to: '16:30',
            id: null
          })]
        };
      });
    }
  }

  registerOnChange(fn: any): void {
    this.controlOnChange = fn;
  }

  registerOnTouched(fn: any): void {
  }

  salesPerson(text: string) {
    if (text) {
      this.userService.searchSalesPerson(text).pipe(
        debounceTime(500),
        distinctUntilChanged((x, y) => x === y),
        tap((_) => this.supervisorsSelect = []),
        tap((e) => e.map((c) => this.supervisorsSelect = [...this.supervisorsSelect, { id: c.id, name: c.name }])),
      ).subscribe();
    } else {
      return;
    }
  }

  usersOffices(text: string) {
    {
      this.userService.searchUsersOffices(text, this.companyId).pipe(
        debounceTime(500),
        distinctUntilChanged((x, y) => x === y),
        tap((_) => this.usersOfficesSelect = []),
        tap((e) => e.map((c) => this.usersOfficesSelect = [...this.usersOfficesSelect, { id: c.id, name: c.name }])),
      ).subscribe();
    }

  }

}
