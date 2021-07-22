import { Component, OnInit, forwardRef, OnDestroy, Input } from '@angular/core';
import { FormGroup, FormArray, FormBuilder, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { IUserNotificationViewModel, IUserNotificationModel, IUserNotificationDataModel } from '../../models/users.models';
import { Subscription } from 'rxjs';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'butor-notifications',
  templateUrl: './notifications.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: forwardRef(() => NotificationsComponent),
    }
  ]
})
export class NotificationsComponent implements OnInit, OnDestroy, ControlValueAccessor {
  @Input() model: IUserNotificationViewModel[] = [];
  notificationsFormSubscription: Subscription;
  notificationsForm: FormArray;
  onChangeControl: (obj: any) => void;
  onTouchedControl: () => void;
  constructor(private formBuilder: FormBuilder) { }

  ngOnInit() {
    if (this.model) {
      this.buildForm();
    }
  }
  private buildForm(): void {
    if (this.notificationsFormSubscription) {
      this.notificationsFormSubscription.unsubscribe();
      this.notificationsFormSubscription = undefined;
    }
    this.notificationsForm = this.formBuilder.array(
      this.model.map((m: IUserNotificationViewModel) => {
        return this.formBuilder.group({
          label: m.label,
          notifications: this.formBuilder.array(
            m.notifications.map((c: IUserNotificationModel) => {
              return this.formBuilder.group({
                id: c.id,
                name: c.name,
                isChecked: c.isChecked
              });
            })
          )
        });
      })
    );
    this.notificationsFormSubscription = this.notificationsForm.valueChanges.pipe(
      tap((val: IUserNotificationViewModel[]) => {
        const filter = val.map(c =>
          c.notifications.filter(d => d.isChecked));
        let result: IUserNotificationDataModel = {
          notificationType: [],
          eventTypeIds: []
        };
        filter.map((c) => c.map(name => name.id ? result.eventTypeIds.push(name.id) : result.notificationType.push(name.name)));

        this.onChangeControl(result);
      }),
      tap(val => this.model = val)
    ).subscribe();
  }
  ngOnDestroy(): void {
    this.notificationsFormSubscription.unsubscribe();

  }

  writeValue(value: IUserNotificationDataModel): void {
    if (value) {
      value.eventTypeIds.map((id) =>
        this.model = this.model.map((p) => ({
          label: p.label,
          notifications:
            p.notifications.map(c => c.id === id
              ? ({ ...c, isChecked: true })
              : c)

        })
        ));
      value.notificationType.map((name) =>
        this.model = this.model.map((c) => ({
          label: c.label,
          notifications: c.notifications.map(notification => notification.name === name
            ? ({ ...notification, isChecked: true })
            : notification)
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
}
