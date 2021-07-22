import { Action } from '@ngrx/store';
import { ICargoFilterModel } from '../../models/cargo.model';

export enum ActionTypes {
    ChangeFilter = 'CARGO:CARGOLIST:CHANGE_FILTER',
    DeleteFilter = 'CARGO:CARGOLIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: ICargoFilterModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type CargoListAction = ChangeFilter
    | DeleteFilter;
