import { Action } from '@ngrx/store';
import { ICncMachinesFilterViewModel } from '../../models/cnc_machines.model';

export enum ActionTypes {
    ChangeFilter = 'CNCMACHINES:CNCMACHINELIST:CHANGE_FILTER',
    DeleteFilter = 'CNCMACHINES:CNCMACHINELIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: ICncMachinesFilterViewModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type CncMachinesAction = ChangeFilter
    | DeleteFilter;
