import { Action } from '@ngrx/store';
import { IGeneralExpensesListFilterViewModel } from '../../models/general-expenses.model';

export enum ActionTypes {
    ChangeFilter = 'GENERALEXPENSES:GENERALEXPENSESLIST:CHANGE_FILTER',
    DeleteFilter = 'GENERALEXPENSES:GENERALEXPENSESLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IGeneralExpensesListFilterViewModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type GeneralExpensesAction = ChangeFilter | DeleteFilter;
