import { createFeatureSelector, createSelector } from "@ngrx/store";
import { IProductsFeatureState } from "../state";
import { IProductsFilterViewModel } from "../../models/products.models";

export const productsSelector = createFeatureSelector<any, IProductsFeatureState>('products');

export const productFilters = createSelector(
    productsSelector,
    (state): IProductsFilterViewModel => state.productList.filters
);

