import { IStatisticsFilterViewModel } from '../../models/statistics';
import { ActionTypes, StatisticsAction } from '../actions/statistics.actions';
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';


const initialState: IStatisticsFilterViewModel = {
    name: '',
    from: undefined,
    to: undefined,
    orderings: [],
    pager: new PagerModel()
};

function statisticsListFilterReducer(
    state: IStatisticsFilterViewModel = initialState,
    action: StatisticsAction
): IStatisticsFilterViewModel {
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

export const statisticsListReducers = combineReducers({
    filters: statisticsListFilterReducer
});
