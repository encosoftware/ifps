import { Action } from '@ngrx/store';
import { ICargoFilterListViewModel } from '../../models/cargo.model';

export enum ActionTypes {
    ChangeFilter = 'CARGO:CARGOLIST:CHANGE_FILTER',
    DeleteFilter = 'CARGO:CARGOLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: ICargoFilterListViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type CargoAction = ChangeFilter | DeleteFilter;
