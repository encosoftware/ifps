import { Action } from '@ngrx/store';
import { IFoilsFilterModel } from '../../models/foils.model';

export enum ActionTypes {
    ChangeFilter = 'FOILS:FOILLIST:CHANGE_FILTER',
    DeleteFilter = 'FOILS:FOILLIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IFoilsFilterModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type FoilsAction = ChangeFilter
    | DeleteFilter;
