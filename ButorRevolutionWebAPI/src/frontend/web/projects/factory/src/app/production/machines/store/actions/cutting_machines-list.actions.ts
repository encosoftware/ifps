import { Action } from '@ngrx/store';
import { ICuttingMachinesFilterViewModel } from '../../models/cutting_machines.model';

export enum ActionTypes {
    ChangeFilter = 'CUTTINGMACHINES:CUTTINGMACHINELIST:CHANGE_FILTER',
    DeleteFilter = 'CUTTINGMACHINES:CUTTINGMACHINELIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: ICuttingMachinesFilterViewModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type CuttingMachinesAction = ChangeFilter
    | DeleteFilter;
