import { ActionTypes, UsersAction } from '../actions/user-list.actions';
import { IUsersFilterViewModel } from '../../models/users.models';
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';

const initialState: IUsersFilterViewModel = {
    name: '',
    role: undefined,
    status: undefined,
    company: '',
    email: '',
    phone: '',
    addedOnTo: undefined,
    pager: new PagerModel()
};

function userListFilterReducer(
    state: IUsersFilterViewModel = initialState,
    action: UsersAction
): IUsersFilterViewModel {

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

export const usersListReducers = combineReducers({
    filters: userListFilterReducer
});
