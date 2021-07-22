import { Action } from '@ngrx/store';
import { IEdgebandingListFilterViewModel } from '../../models/edgebanding.model';

export enum ActionTypes {
    ChangeFilter = 'EDGEBANDINGS:EDGEBANDINGLIST:CHANGE_FILTER',
    DeleteFilter = 'EDGEBANDINGS:EDGEBANDINGLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IEdgebandingListFilterViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type EdgebandingsAction = ChangeFilter | DeleteFilter;
