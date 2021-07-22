import { Action } from '@ngrx/store';
import { IPackingListFilterViewModel } from '../../models/packing.model';

export enum ActionTypes {
    ChangeFilter = 'PACKING:PACKINGLIST:CHANGE_FILTER',
    DeleteFilter = 'PACKING:PACKINGLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IPackingListFilterViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type PackingAction = ChangeFilter | DeleteFilter;
