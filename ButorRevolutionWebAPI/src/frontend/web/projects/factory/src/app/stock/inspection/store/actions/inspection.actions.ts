import { Action } from '@ngrx/store';
import { IInspectionFilterListViewModel } from '../../models/inspection.model';

export enum ActionTypes {
    ChangeFilter = 'INSPECTION:INSPECTIONLIST:CHANGE_FILTER',
    DeleteFilter = 'INSPECTION:INSPECTIONLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IInspectionFilterListViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type InspectionAction = ChangeFilter | DeleteFilter;
