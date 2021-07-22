import { combineReducers } from '@ngrx/store';
import { ITrendFilterModel } from '../../models/trends.models';
import { TrendsAction, ActionTypes } from '../actions/trend-list.actions';

const initialState: ITrendFilterModel = {
    intervalFrom: new Date(2019, 11, 31),
    intervalTo: new Date(2020, 11, 31),
    take: 10
};

function trendListFilterReducer(
    state: ITrendFilterModel = initialState,
    action: TrendsAction
): ITrendFilterModel {
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

export const trendsListReducer = combineReducers({
    filters: trendListFilterReducer
});
