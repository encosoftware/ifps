import { Action } from '@ngrx/store';
import { IUsersFilterViewModel } from '../../models/users.models';

export enum ActionTypes {
    ChangeFilter = 'USERS:USERLIST:CHANGE_FILTER',
    DeleteFilter = 'USERS:USERLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IUsersFilterViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type UsersAction = ChangeFilter | DeleteFilter;
