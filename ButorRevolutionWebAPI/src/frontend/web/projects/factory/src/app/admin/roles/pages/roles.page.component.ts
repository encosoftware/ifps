import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SaveAsComponent } from '../components/save-as/save-as.component';
import { RolesService } from '../services/roles.service';
import { IModule } from '../models/claims.model';
import { INewRoleViewModel, IDivisionViewModel } from '../models/roles.model';
import { switchMap } from 'rxjs/operators';
import { SnackbarService } from 'butor-shared-lib';
import { ClaimPolicyEnum } from '../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';

@Component({
  templateUrl: './roles.page.component.html',
  styleUrls: ['./roles.page.component.scss']
})
export class RolesPageComponent implements OnInit {

  isLoading = false;
  selectedId: number;
  claims: IModule[];
  newRole: INewRoleViewModel;
  roles: IDivisionViewModel[];
  claimPolicyEnum = ClaimPolicyEnum;

  constructor(
    public dialog: MatDialog,
    private rolesService: RolesService,
    public snackBar: SnackbarService,
    private translate: TranslateService
  ) { }

  ngOnInit() {
    this.isLoading = true;
    this.rolesService.getRoles().subscribe(res => {
      this.roles = res;
    },
      () => { },
      () => this.isLoading = false);
  }

  onRoleSelected(id: number) {
    this.selectedId = id;
    this.rolesService.getClaimsList().pipe(
      switchMap(res => {
        this.claims = res;
        return this.rolesService.getClaimsForRole(id);
      })
    ).subscribe(result => this.setSelectedClaims(result));
  }

  private setSelectedClaims(ids: number[]): void {
    for (let modules of this.claims) {
      for (let claim of modules.claims) {
        if (ids.includes(claim.id)) {
          claim.enabled = true;
        }
      }
      let claimIsAllTrue = modules.claims.every((value) => this.isAllTrue(value.enabled));
      let claimIsAllFalse = modules.claims.every((value) => this.isAllFalse(value.enabled));
      if (claimIsAllTrue) {
        modules.checkmarkClass = 'form__checkbox-button';
        modules.enabled = true;
      }
      if (claimIsAllFalse) {
        modules.checkmarkClass = 'form__checkbox-button-line';
        modules.enabled = false;
      }
      if (!claimIsAllFalse && !claimIsAllTrue) {
        modules.checkmarkClass = 'form__checkbox-button-line';
        modules.enabled = true;
      }
    }
  }

  isAllTrue(currentValue) {
    return currentValue === true;
  }
  isAllFalse(currentValue) {
    return currentValue === false;
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(SaveAsComponent, {
      width: '48rem'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.isLoading = true;
        this.rolesService.postRole(result, this.claims).subscribe(() => {
          this.rolesService.getRoles().subscribe(res => this.roles = res);
        },
          () => { },
          () => this.isLoading = false);
      }
    });
  }

  saveRole() {
    this.isLoading = true;
    this.rolesService.putRole(this.selectedId, this.claims).subscribe(res => {
      this.snackBar.customMessage(this.translate.instant('snackbar.saved'));
    },
      () => { },
      () => this.isLoading = false);
  }

}
