import { Action } from '@ngrx/store';
import { IAppliancesFilterModel } from '../../models/appliences.model';

export enum ActionTypes {
    ChangeFilter = 'APPLIANCES:APPLIANCELIST:CHANGE_FILTER',
    DeleteFilter = 'APPLIANCES:APPLIANCELIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IAppliancesFilterModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type AppliancesAction = ChangeFilter
    | DeleteFilter;
