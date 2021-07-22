import { ActionReducer, combineReducers, compose } from '@ngrx/store';
import { IProductsFeatureState } from '../state/index';
import { productsListReducers } from '../reducers/products-list.reducers';

export const productsReducers = combineReducers({
    productList: productsListReducers
});
