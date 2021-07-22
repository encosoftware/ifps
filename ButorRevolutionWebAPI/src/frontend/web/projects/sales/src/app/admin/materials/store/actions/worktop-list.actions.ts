import { Action } from '@ngrx/store';
import { IWorktopFilterModel } from '../../models/worktops.model';


export enum ActionTypes {
    ChangeFilter = 'WORKTOPS:WORKTOPLIST:CHANGE_FILTER',
    DeleteFilter = 'WORKTOPS:WORKTOPLIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IWorktopFilterModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type WorktopsAction = ChangeFilter
    | DeleteFilter;
