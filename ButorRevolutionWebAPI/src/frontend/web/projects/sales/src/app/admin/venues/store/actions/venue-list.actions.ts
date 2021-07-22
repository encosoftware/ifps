import { Action } from '@ngrx/store';
import { IVenueFilterListViewModel } from '../../models/venues.model';

export enum ActionTypes {
    ChangeFilter = 'VENUES:VENUELIST:CHANGE_FILTER',
    DeleteFilter = 'VENUES:VENUELIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IVenueFilterListViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type VenuesAction = ChangeFilter | DeleteFilter;
