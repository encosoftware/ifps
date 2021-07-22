import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IModule, IClaim } from '../models/claims.model';
import { INewRoleViewModel, IDivisionViewModel, ICategoryShortViewModel } from '../models/roles.model';
import {
    ApiRolesClient,
    RoleListDto,
    RoleUpdateDto,
    RoleCreateDto,
    RoleDetailsDto,
    ClaimGroupDto,
    GroupingCategoryEnum,
    ApiClaimsClient,
    ApiGroupingcategoriesHierarchicalListClient,
    ApiClaimsGroupsClient,
    ClaimPolicyEnum,
    ApiDivisionsClient
} from '../../../shared/clients';
import { map, tap } from 'rxjs/operators';
import { IDropDownViewModel } from '../../../sales/appointments/models/appointments.model';

@Injectable({
    providedIn: 'root'
})
export class RolesService {

    constructor(
        private rolesClient: ApiRolesClient,
        private claimsClient: ApiClaimsGroupsClient,
        private categoryClient: ApiGroupingcategoriesHierarchicalListClient,
        private divisionClient: ApiDivisionsClient
    ) { }

    postRole(newRole: INewRoleViewModel, moduls: IModule[]): Observable<number> {
        let dto = new RoleCreateDto();
        dto.name = newRole.name;
        dto.divisionId = newRole.categoryId;
        dto.claimIdList = [];
        for (let modul of moduls) {
            for (let claim of modul.claims) {
                if (claim.enabled) {
                    dto.claimIdList.push(claim.id);
                }
            }
        }
        return this.rolesClient.postRole(dto);
    }

    getClaimsList(): Observable<IModule[]> {
        return this.claimsClient.getClaimsWithGroupsList().pipe(map(this.claimsToViewModel));
    }

    private claimsToViewModel(dto: ClaimGroupDto[]): IModule[] {
        let modules: IModule[] = [];
        for (let moduleDto of dto) {
            let module: IModule = {
                claims: [],
                name: moduleDto.division,
                detail: moduleDto.detail,
                checkmarkClass: 'form__checkbox-line',
                enabled: false
            };
            for (let claimDto of moduleDto.claims) {
                let claim: IClaim = {
                    enabled: false,
                    id: claimDto.id,
                    name: ClaimPolicyEnum[ClaimPolicyEnum[claimDto.name]],
                };
                module.claims.push(claim);
            }
            modules.push(module);
        }
        return modules;
    }

    getClaimsForRole(roleId: number): Observable<number[]> {
        return this.rolesClient.getRoleById(roleId).pipe(map(this.rolebyIdToViewModel));
    }

    private rolebyIdToViewModel(dto: RoleDetailsDto): number[] {
        let ids = [];
        for (let id of dto.claimIds) {
            ids.push(id);
        }
        return ids;
    }

    putRole(roleId: number, moduls: IModule[]): Observable<void> {
        let dto = new RoleUpdateDto();
        dto.claimIdList = [];
        for (let modul of moduls) {
            for (let claim of modul.claims) {
                if (claim.enabled) {
                    dto.claimIdList.push(claim.id);
                }
            }
        }
        return this.rolesClient.updateRole(roleId, dto);
    }

    getRoles(): Observable<IDivisionViewModel[]> {
        return this.rolesClient.getRoles().pipe(map(this.rolesDtoToViewModel));
    }

    private rolesDtoToViewModel(dto: RoleListDto[]): IDivisionViewModel[] {
        let divisions: IDivisionViewModel[] = [];
        for (let role of dto) {
            let found = false;
            for (let div of divisions) {
                if (role.divisionId === div.id) {
                    found = true;
                    div.roles.push({ id: role.id, name: role.name });
                }
            }
            if (found === false) {
                divisions.push({
                    id: role.divisionId,
                    name: role.divisionName,
                    roles: [{ id: role.id, name: role.name }]
                });
            }
        }
        return divisions;
    }

    getCategories(): Observable<ICategoryShortViewModel[]> {
        return this.categoryClient.getHierarchicalCategories(GroupingCategoryEnum.RoleType, false);
    }

    getDivisionDropDown(): Observable<IDropDownViewModel[]> {
        return this.divisionClient.getDivisionList();
    }

}
