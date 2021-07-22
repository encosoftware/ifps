
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IOrderSchedulingListFilterViewModel } from '../../models/order-scheduling.model';
import { OrderSchedulingsAction, ActionTypes } from '../actions/order-schedulings.actions';

const initialState: IOrderSchedulingListFilterViewModel = {
    completion: undefined,
    deadline: undefined,
    orderId: '',
    statusId: undefined,
    workingNr: '',
    pager: new PagerModel()
};

function orderSchedulingListFilterReducer(
    state: IOrderSchedulingListFilterViewModel = initialState,
    action: OrderSchedulingsAction
): IOrderSchedulingListFilterViewModel {

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

export const orderSchedulingListReducers = combineReducers({
    filters: orderSchedulingListFilterReducer
});
