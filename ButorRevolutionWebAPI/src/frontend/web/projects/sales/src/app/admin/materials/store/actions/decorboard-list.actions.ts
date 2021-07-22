import { Action } from '@ngrx/store';
import { IDecorboardFilterModel } from '../../models/decorboards.model';

export enum ActionTypes {
    ChangeFilter = 'DECORBOARDS:DECORBOARDSLIST:CHANGE_FILTER',
    DeleteFilter = 'DECORBOARDS:DECORBOARDSLIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IDecorboardFilterModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type DecoboardsAction = ChangeFilter
    | DeleteFilter;
