
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { ICellFilterListViewModel } from '../../models/cells.model';
import { CellsAction, ActionTypes } from '../actions/cells.actions';

const initialState: ICellFilterListViewModel = {
    description: '',
    name: '',
    stock: '',
    pager: new PagerModel()
};

function cellListFilterReducer(
    state: ICellFilterListViewModel = initialState,
    action: CellsAction
): ICellFilterListViewModel {

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

export const cellListReducers = combineReducers({
    filters: cellListFilterReducer
});
