import { IProductsFilterViewModel } from '../../models/products.models';
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { ProductsAction, ActionTypes } from '../actions/products-list.actions';

const initialState: IProductsFilterViewModel = {
    description: '',
    code: '',
    category: '',
    categoryId: undefined,
    pager: new PagerModel()
};


function productListFilterReducer(
    state: IProductsFilterViewModel = initialState,
    action: ProductsAction
): IProductsFilterViewModel {

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

export const productsListReducers = combineReducers({
    filters: productListFilterReducer
});
