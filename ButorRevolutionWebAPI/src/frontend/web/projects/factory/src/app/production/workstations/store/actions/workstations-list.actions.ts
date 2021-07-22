import { IWorkstationsFilterModel } from '../../models/workstations.model';
import { Action } from '@ngrx/store';

export enum ActionTypes {
    ChangeFilter = 'WORKSTATIONS:WORKSTATIONLIST:CHANGE_FILTER',
    DeleteFilter = 'WORKSTATIONS:WORKSTATIONLIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IWorkstationsFilterModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type WorkstationsAction = ChangeFilter
    | DeleteFilter;
