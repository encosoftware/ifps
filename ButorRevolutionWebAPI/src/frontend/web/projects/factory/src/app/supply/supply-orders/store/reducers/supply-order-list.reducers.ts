import { ISupplyOrderFilterViewModel } from '../../models/supply-orders.model';
import { PagerModel } from '../../../../shared/models/pager.model';
import { SupplyOrdersAction, ActionTypes } from '../actions/supply-order-list.actions';
import { combineReducers } from '@ngrx/store';

const initialState: ISupplyOrderFilterViewModel = {
    deadline: null,
    material: '',
    name: '',
    orderId: '',
    supplier: undefined,
    workingNumber: '',
    pager: new PagerModel()
};

function supplyOrderFilterReducer(
    state: ISupplyOrderFilterViewModel = initialState,
    action: SupplyOrdersAction
): ISupplyOrderFilterViewModel {
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

export const supplyOrderListReducer = combineReducers({
    filters: supplyOrderFilterReducer
});
