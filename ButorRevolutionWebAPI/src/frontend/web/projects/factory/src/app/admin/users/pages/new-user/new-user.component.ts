import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UsersService } from '../../services/users.service';
import {
  IUserEditViewModel,
  IModuleViewModel,
  IUserNotificationViewModel,
  IUserWorkingHoursModel,
  DayTypeListModel,
  IUserBasicInfoViewModel
} from '../../models/users.models';
import { map, tap, switchMap, finalize, catchError, filter } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { uniq } from 'ramda';
import { NgForm } from '@angular/forms';
import { SnackbarService } from 'butor-shared-lib';
import { ClaimPolicyEnum } from '../../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';

@Component({
  templateUrl: './new-user.component.html',
  styleUrls: ['./new-user.component.scss']
})
export class NewUserComponent implements OnInit {

  isLoading = false;
  workingHours: IUserWorkingHoursModel[];
  editUser: IUserEditViewModel;
  notifications: IUserNotificationViewModel[];
  claims: IModuleViewModel[];
  id: number;
  isTemplateAppears: string[] = [];
  workingInfo: DayTypeListModel[] = [];
  roleClaimsPrev: number[] = [];
  userDetails: Subscription;
  error: string;
  workingInfoAppear = 1;
  claimPolicyEnum = ClaimPolicyEnum;

  protected templateSubscription: Subscription;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UsersService,
    public snackBar: SnackbarService,
    public translate: TranslateService
  ) {

  }

  ngOnInit() {

    this.editUser = {
      basicInfo: {
        id: 1,
        name: '',
        language: null,
      },
      isActivated: null,
      isEmployee: null,
      claims: [
      ],
      workingInfo: {
        usersOffices: [],
        supervisors: null,
        supervisees: [
          {
            id: null,
            name: null
          }
        ],
        groups: [
          ''
        ],
        discount: { from: null, to: null },

      },
      daysoff: {
        sickLeave: [],
        daysOff: [],
        deleted: []
      },
      notifications: {
        notificationType: [],
        eventTypeIds: []
      }
    };
    this.claims = [];
    this.notifications = [{
      label: 'Recieve notifications via',
      notifications: [
        {
          isChecked: false,
          name: 'Email',
        },
        {
          isChecked: false,
          name: 'PushNotification',
        },
        {
          isChecked: false,
          name: 'SMS',
        },
      ]
    },
    {
      label: 'Notify me about',
      notifications: []
    }];
    this.workingHours = [];
    this.id = +this.route.snapshot.paramMap.get('id');
    this.userService.getUserEdit(this.id).pipe(
      map(user => {
        this.editUser.basicInfo = user.basicInfo;
        this.editUser.claims = user.claims;
        this.editUser.notifications = user.notifications;
        this.editUser.workingInfo = user.workingInfo;
        this.editUser.isActivated = user.isActivated;
        this.editUser.isEmployee = user.isEmployee;
      }),
      switchMap((ins) =>
        this.userService.getDaytypes().pipe(
          tap(workingH => {
            this.workingInfo = workingH;
          })
        )
      ),
      switchMap((ins) =>
        this.userService.getClaimsList().pipe(
          tap(resp => {
            this.claims = resp;
          })
        )
      ),
      finalize(() => { this.isLoading = true; })

    ).subscribe();
  }

  isTemplateLoad(event: string[]) {
    this.isTemplateAppears = [...event];
    if (this.isTemplateAppears.includes('Sales') && this.workingInfoAppear === 1) {
      this.workingInfoAppear++;
      this.userService.getDaytypes().pipe(
        tap(workingH => {
          this.workingInfo = [...workingH];
        })
      ).subscribe();
    }
  }

  basicInfoChange(event: IUserBasicInfoViewModel) {
    if (this.roleClaimsPrev
      && event.roles !== this.roleClaimsPrev
      && event.roles.length) {
      this.userService.getRolesByid(event.roles[event.roles.length - 1]).pipe(
        tap((c) => this.editUser.claims = uniq([...this.editUser.claims, ...c.claimIds]))
      ).subscribe();

    }

    this.roleClaimsPrev = event.roles;
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.userService.updateUsers(this.id, this.editUser).pipe(
        tap(() => this.isLoading = false),
        switchMap((days) =>
          this.userService.putAbsenceUsersDay(this.id, this.editUser.daysoff).pipe(
            map(res => res),
            catchError(() => this.error = 'Error: could not load data'),
            // finalize(() => this.isLoading = true)
          )
        ),
        filter(val => !!this.editUser.daysoff.deleted),
        switchMap((deleted) =>
          this.userService.deleteAbsenceUsersDay(this.id, this.editUser.daysoff.deleted).pipe(
            map(res => res),
            catchError(() => this.error = 'Error: could not load data'),
          )
        ),
        switchMap((get) =>
          this.userService.getUserEdit(this.id).pipe(
            map(user => {
              this.editUser.isEmployee = user.isEmployee;
            }),
            catchError(() => this.error = 'Error: could not load data'),
            finalize(() => { this.isLoading = true; })
          )
        ),
        finalize(() => {
          this.snackBar.customMessage(this.translate.instant('snackbar.saved'));
        })
      ).subscribe();
    } else {

      switch (true) {
        case form.controls.basicInfo.invalid: {
          this.snackBar.customMessage(this.translate.instant('snackbar.basicInfoInvalid'));
          break;
        }
        case form.controls.workingInfo.invalid: {
          this.snackBar.customMessage(this.translate.instant('snackbar.workingInfoInvalid'));
          break;
        }
        case form.controls.claims.invalid: {
          this.snackBar.customMessage(this.translate.instant('snackbar.claimsInvalid'));
          break;
        }
        default: {
          this.snackBar.customMessage(this.error);
          break;
        }
      }
    }
  }

  activateToggle() {
    if (this.editUser.isActivated) {
      this.isLoading = false;
      this.userService.putDeactivate(this.id)
        .subscribe(res => { this.editUser.isActivated = !this.editUser.isActivated; this.isLoading = true; });
    } else {
      this.isLoading = false;
      this.userService.putAactivate(this.id)
        .subscribe(res => { this.editUser.isActivated = !this.editUser.isActivated; this.isLoading = true; });
    }
  }

  deleteUser() {
    this.userService.deleteUsers(this.id).pipe(
      tap(() => this.router.navigate([`/admin/users/`]))
    ).subscribe();
  }

  notinclude(value: Array<string>, arg: string): boolean {
    let trueOrFalse: boolean;
    if (value.length === 1) {
      trueOrFalse = !value.includes(arg);
    } else {
      return true;
    }

    return trueOrFalse;
  }
}
