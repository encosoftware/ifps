import { Action } from '@ngrx/store';
import { ICameraFilterModel } from '../../models/cameras.model';

export enum ActionTypes {
    ChangeFilter = 'CAMERAS:CAMERALIST:CHANGE_FILTER',
    DeleteFilter = 'CAMERAS:CAMERALIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: ICameraFilterModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type CamerasAction = ChangeFilter
    | DeleteFilter;
