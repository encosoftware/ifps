import { Action } from '@ngrx/store';
import { IAssemblyListFilterViewModel } from '../../models/assembly.model';

export enum ActionTypes {
    ChangeFilter = 'ASSEMBLIES:ASSEMBLYLIST:CHANGE_FILTER',
    DeleteFilter = 'ASSEMBLIES:ASSEMBLYLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IAssemblyListFilterViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type AssemblyAction = ChangeFilter | DeleteFilter;
