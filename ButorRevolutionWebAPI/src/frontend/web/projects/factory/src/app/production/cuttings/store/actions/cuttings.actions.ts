import { Action } from '@ngrx/store';
import { ICuttingListFilterViewModel } from '../../models/cuttings.model';

export enum ActionTypes {
    ChangeFilter = 'CUTTINGS:CUTTINGLIST:CHANGE_FILTER',
    DeleteFilter = 'CUTTINGS:CUTTINGLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: ICuttingListFilterViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type CuttingsAction = ChangeFilter | DeleteFilter;
