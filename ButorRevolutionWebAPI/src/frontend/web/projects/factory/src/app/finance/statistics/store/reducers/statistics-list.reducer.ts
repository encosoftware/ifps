import { combineReducers } from '@ngrx/store';
import { IStatisticsFilterModel } from '../../models/statistics.model';
import { StatisticsAction, ActionTypes } from '../actions/statistics-list.actions';

const initialState: IStatisticsFilterModel = {
    year: 2020
};

function statisticsListFilterReducer(
    state: IStatisticsFilterModel = initialState,
    action: StatisticsAction
): IStatisticsFilterModel {
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

export const statisticsListReducer = combineReducers({
    filters: statisticsListFilterReducer
});
