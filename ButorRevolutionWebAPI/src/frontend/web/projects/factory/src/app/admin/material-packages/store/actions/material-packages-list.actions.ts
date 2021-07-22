import { Action } from '@ngrx/store';
import { IMaterialPackageFilterModel } from '../../models/material-packages.model';

export enum ActionTypes {
    ChangeFilter = 'MATERIALPACKAGES:MATERIALPACKAGELIST:CHANGE_FILTER',
    DeleteFilter = 'MATERIALPACKAGES:MATERIALPACKAGELIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IMaterialPackageFilterModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type MaterialPackageAction = ChangeFilter
    | DeleteFilter;
