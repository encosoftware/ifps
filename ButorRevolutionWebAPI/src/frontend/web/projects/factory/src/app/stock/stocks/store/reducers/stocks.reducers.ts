
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IStorageFilterListViewModel } from '../../models/stocks.model';
import { StorageAction, ActionTypes } from '../actions/stocks.actions';


const initialState: IStorageFilterListViewModel = {
    name: '',
    address: '',
    pager: new PagerModel()
};

function stockListFilterReducer(
    state: IStorageFilterListViewModel = initialState,
    action: StorageAction
): IStorageFilterListViewModel {

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

export const stockListReducers = combineReducers({
    filters: stockListFilterReducer
});
