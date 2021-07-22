import { Action } from '@ngrx/store';
import { ISortingListFilterViewModel } from '../../models/sorting.model';

export enum ActionTypes {
    ChangeFilter = 'SORTING:SORTINGLIST:CHANGE_FILTER',
    DeleteFilter = 'SORTING:SORTINGLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: ISortingListFilterViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type SortingAction = ChangeFilter | DeleteFilter;
