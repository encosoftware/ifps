import { combineReducers } from '@ngrx/store';
import { packingListReducers } from './packing.reducer';

export const packingReducers = combineReducers({
    packingList: packingListReducers
});
