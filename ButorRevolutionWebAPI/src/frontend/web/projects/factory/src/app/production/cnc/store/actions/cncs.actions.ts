import { Action } from '@ngrx/store';
import { ICncListFilterViewModel } from '../../models/cnc.model';

export enum ActionTypes {
    ChangeFilter = 'CNCS:CNCLIST:CHANGE_FILTER',
    DeleteFilter = 'CNCS:CNCLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: ICncListFilterViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type CncsAction = ChangeFilter | DeleteFilter;
