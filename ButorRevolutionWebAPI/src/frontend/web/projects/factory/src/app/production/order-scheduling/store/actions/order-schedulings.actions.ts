import { Action } from '@ngrx/store';
import { IOrderSchedulingListFilterViewModel } from '../../models/order-scheduling.model';

export enum ActionTypes {
    ChangeFilter = 'ORDER-SCHEDULINGS:ORDER-SCHEDULINGLIST:CHANGE_FILTER',
    DeleteFilter = 'ORDER-SCHEDULINGS:ORDER-SCHEDULINGLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IOrderSchedulingListFilterViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type OrderSchedulingsAction = ChangeFilter | DeleteFilter;
