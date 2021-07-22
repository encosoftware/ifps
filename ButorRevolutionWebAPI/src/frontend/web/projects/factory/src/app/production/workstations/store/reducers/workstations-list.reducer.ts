import { IWorkstationsFilterModel } from '../../models/workstations.model';
import { PagerModel } from '../../../../shared/models/pager.model';
import { WorkstationsAction, ActionTypes } from '../actions/workstations-list.actions';
import { combineReducers } from '@ngrx/store';

const initialState: IWorkstationsFilterModel = {
    name: '',
    optimalCrew: undefined,
    machine: undefined,
    type: undefined,
    status: undefined,

    pager: new PagerModel()
};

function workstationsListFilterReducer(
    state: IWorkstationsFilterModel = initialState,
    action: WorkstationsAction
): IWorkstationsFilterModel {
    switch (action.type) {
        case ActionTypes.ChangeFilter:
            return {
                ...state,
                ...action.payload
            };

        case ActionTypes.DeleteFilter:
            return {
                ...initialState
            };
    }

    return state;
}

export const workstationsListReducer = combineReducers({
    filters: workstationsListFilterReducer
});
