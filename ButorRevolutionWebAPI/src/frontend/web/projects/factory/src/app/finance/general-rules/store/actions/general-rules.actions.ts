import { Action } from '@ngrx/store';
import { IGeneralRulesListFilterViewModel } from '../../models/general-rules.model';

export enum ActionTypes {
    ChangeFilter = 'GENERALRULES:GENERALRULESLIST:CHANGE_FILTER',
    DeleteFilter = 'GENERALRULES:GENERALRULESLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IGeneralRulesListFilterViewModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type GeneralRulesAction = ChangeFilter | DeleteFilter;
