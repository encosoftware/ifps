import { Action } from '@ngrx/store';
import { IFurnitureUnitFilterModel } from '../../models/furniture-unit.model';

export enum ActionTypes {
    ChangeFilter = 'FUS:FULIST:CHANGE_FILTER',
    DeleteFilter = 'FUS:FULIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IFurnitureUnitFilterModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type FUAction = ChangeFilter
    | DeleteFilter;
