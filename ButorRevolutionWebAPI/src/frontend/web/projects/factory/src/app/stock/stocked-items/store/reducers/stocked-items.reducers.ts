
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IStockedItemFilterListViewModel } from '../../models/stocked-items.model';
import { StockedItemsAction, ActionTypes } from '../actions/stocked-items.actions';

const initialState: IStockedItemFilterListViewModel = {
    description: '',
    cellMeta: '',
    cellName: '',
    code: '',
    order: '',
    quantity: undefined,
    pager: new PagerModel()
};

function stockedItemListFilterReducer(
    state: IStockedItemFilterListViewModel = initialState,
    action: StockedItemsAction
): IStockedItemFilterListViewModel {

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

export const stockedItemListReducers = combineReducers({
    filters: stockedItemListFilterReducer
});
