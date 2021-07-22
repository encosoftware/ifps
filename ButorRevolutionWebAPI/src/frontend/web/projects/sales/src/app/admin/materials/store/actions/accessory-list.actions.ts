import { Action } from '@ngrx/store';
import { IAccessoryFilterModel } from '../../models/accessories.model';

export enum ActionTypes {
    ChangeFilter = 'ACCESSORIES:ACCESSORIESLIST:CHANGE_FILTER',
    DeleteFilter = 'ACCESSORIES:ACCESSORIESLIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IAccessoryFilterModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type AccessoriesAction = ChangeFilter
     | DeleteFilter;
