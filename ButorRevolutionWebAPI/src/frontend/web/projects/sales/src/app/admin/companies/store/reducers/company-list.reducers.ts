import { ICompanyFilterModel } from '../../models/company.model';
import { PagerModel } from '../../../../shared/models/pager.model';
import { CompaniesAction, ActionTypes } from '../actions/company-list.actions';
import { combineReducers } from '@ngrx/store';
import { CompanyTypeEnum } from '../../../../shared/clients';

const initialState: ICompanyFilterModel = {
    id: null,
    name: '',
    companyType: CompanyTypeEnum.None,
    email: '',
    address: '',
    pager: new PagerModel()
};

function companyListFilterReducer(
    state: ICompanyFilterModel = initialState,
    action: CompaniesAction
): ICompanyFilterModel {
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

export const companiesListReducer = combineReducers({
    filters: companyListFilterReducer
});
