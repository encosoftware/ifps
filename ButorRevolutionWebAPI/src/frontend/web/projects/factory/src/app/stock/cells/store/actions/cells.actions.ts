import { Action } from '@ngrx/store';
import { ICellFilterListViewModel } from '../../models/cells.model';

export enum ActionTypes {
    ChangeFilter = 'CELLS:CELLLIST:CHANGE_FILTER',
    DeleteFilter = 'CELLS:CELLLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: ICellFilterListViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type CellsAction = ChangeFilter | DeleteFilter;
