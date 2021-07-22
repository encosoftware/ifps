
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IVenueFilterListViewModel } from '../../models/venues.model';
import { VenuesAction, ActionTypes } from '../actions/venue-list.actions';


const initialState: IVenueFilterListViewModel = {
    name: '',
    rooms: undefined,
    address: '',
    phone: '',
    email: '',
    pager: new PagerModel()
};

function venueListFilterReducer(
    state: IVenueFilterListViewModel = initialState,
    action: VenuesAction
): IVenueFilterListViewModel {

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

export const venuesListReducers = combineReducers({
    filters: venueListFilterReducer
});
