import { Action } from '@ngrx/store';
import { ICncMachinesFilterViewModel } from '../../models/cnc_machines.model';
import { IEdgingMachinesFilterViewModel } from '../../models/edging_machines.model';

export enum ActionTypes {
    ChangeFilter = 'EDGINGMACHINES:EDGINGMACHINELIST:CHANGE_FILTER',
    DeleteFilter = 'EDGINGMACHINES:EDGINGMACHINELIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IEdgingMachinesFilterViewModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type EdgingMachinesAction = ChangeFilter
    | DeleteFilter;
