import { Component, OnInit, ViewChild } from '@angular/core';
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
import { map, tap, switchMap, finalize, catchError, filter, take } from 'rxjs/operators';
import { Subscription, of } from 'rxjs';
import { uniq } from 'ramda';
import { NgForm } from '@angular/forms';
import { SnackbarService } from 'butor-shared-lib';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../../../core/store/selectors/core.selector';
import { LoginToken } from '../../../../core/store/actions/core.actions';
import { UserPicUpdateService } from '../../../../shared/services/user-pic-update.service';
import { BasicInfoComponent } from '../../components/basic-info/basic-info.component';
import { TranslateService } from '@ngx-translate/core';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  templateUrl: './new-user.component.html',
  styleUrls: ['./new-user.component.scss']
})
export class NewUserComponent implements OnInit {

  @ViewChild(BasicInfoComponent) basicInfoComponent: BasicInfoComponent;
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
  protected templateSubscription: Subscription;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UsersService,
    public snackBar: SnackbarService,
    private store: Store<any>,
    private picUpdateService: UserPicUpdateService,
    public translate: TranslateService
  ) {

  }

  ngOnInit() {
    this.isLoading = false;
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
      switchMap(() =>
        this.userService.getDaytypes().pipe(
          tap(workingH => {
            this.workingInfo = workingH;
          })
        )
      ),
      switchMap(() =>
        this.userService.getClaimsList().pipe(
          tap(resp => {
            this.claims = resp;
          })
        )
      ),
      switchMap(() =>
        this.userService.getNotificationEvent().pipe(
          tap(resp => {
            this.notifications[1].notifications.push(...resp);
          })
        )
      ),
      finalize(() => { this.isLoading = true; })
    ).subscribe();
  }

  isTemplateLoad(event: string[]) {
    this.isTemplateAppears = event.map(e => e);
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
    this.userService.getDetailedRolesList(event.roles).subscribe(
      res => this.isTemplateLoad(res)
    );
    this.roleClaimsPrev = event.roles;
  }

  onSubmit(form: NgForm) {
    this.basicInfoComponent.submitted = true;
    if (form.valid) {
      this.userService.updateUsers(this.id, this.editUser).pipe(
        tap(() => this.isLoading = false),
        filter(() => !!(!this.isTemplateAppears.includes('Customer'))),
        switchMap(() =>
          this.userService.putAbsenceUsersDay(this.id, this.editUser.daysoff).pipe(
            map(res => res),
            catchError(() => this.error = 'Error: could not load data'),
          )
        ),
        filter(() => !!this.editUser.daysoff.deleted),
        switchMap(() =>
          this.userService.deleteAbsenceUsersDay(this.id, this.editUser.daysoff.deleted).pipe(
            map(res => res),
            catchError(() => this.error = 'Error: could not load data'),
          )
        ),
        switchMap(() =>
          this.userService.getUserEdit(this.id).pipe(
            map(user => {
              this.editUser.basicInfo = user.basicInfo;
              this.editUser.claims = user.claims;
              this.editUser.notifications = user.notifications;
              this.editUser.workingInfo = user.workingInfo;
              this.editUser.isActivated = user.isActivated;
              this.editUser.isEmployee = user.isEmployee;
            }),
            catchError(() => this.error = 'Error: could not load data')
          )
        ),
        switchMap(() =>
          this.store.pipe(
            select(coreLoginT),
            take(1),
            tap((resp) => {
              if (+resp.UserId === this.id) {
                this.store.dispatch(new LoginToken({
                  accessToken: resp.accessToken,
                  refreshToken: resp.refreshToken,
                  Email: resp.Email,
                  ImageContainerName: this.editUser.basicInfo.image.containerName,
                  ImageFileName: this.editUser.basicInfo.image.fileName,
                  Language: resp.Language,
                  RoleName: resp.RoleName,
                  UserName: resp.UserName,
                  IFPSClaim: resp.IFPSClaim,
                  UserId: resp.UserId,
                  CompanyId: this.editUser.basicInfo.company.toString()
                }));
                this.picUpdateService.updateUserPic(this.editUser.basicInfo.image.containerName, this.editUser.basicInfo.image.fileName);
              }
            })
          )
        ),
        finalize(() => {
          this.snackBar.customMessage(this.translate.instant('snackbar.saved'));
          this.isLoading = true;
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
      catchError((err: HttpErrorResponse) => {
        if (err.status === 400) {
          this.snackBar.customMessage(this.translate.instant('snackbar.userCantDelete'));
        }
        return of();
      })

    ).subscribe(res => {
      this.snackBar.customMessage(this.translate.instant('snackbar.deleted'));
      this.router.navigate([`/admin/users/`]);
    });
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
