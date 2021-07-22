import { Action } from '@ngrx/store';
import { ICompanyFilterModel } from '../../models/company.model';

export enum ActionTypes {
    ChangeFilter = 'COMPANIES:COMPANYLIST:CHANGE_FILTER',
    DeleteFilter = 'COMPANIES:COMPANYLIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: ICompanyFilterModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type CompaniesAction = ChangeFilter
    | DeleteFilter;
