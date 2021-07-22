
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IAssemblyListFilterViewModel } from '../../models/assembly.model';
import { AssemblyAction, ActionTypes } from '../actions/assembly.actions';

const initialState: IAssemblyListFilterViewModel = {
    orderId: '',
    unitId: '',
    workingNr: '',
    date: undefined,
    workerName: '',
    pager: new PagerModel()
};

function assemblyListFilterReducer(
    state: IAssemblyListFilterViewModel = initialState,
    action: AssemblyAction
): IAssemblyListFilterViewModel {

    switch (action.type) {
        case ActionTypes.ChangeFilter:
            return {
                ...state,
                ...action.payload
            };
        case ActionTypes.DeleteFilter:
            return { ...initialState };
    }

    return state;
}

export const assemblyListReducers = combineReducers({
    filters: assemblyListFilterReducer
});
