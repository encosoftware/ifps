import { Action } from '@ngrx/store';
import { IProductsFilterViewModel } from '../../models/products.models';

export enum ActionTypes {
    ChangeFilter = 'PRODUCTS:PRODUCTSLIST:CHANGE_FILTER',
    DeleteFilter = 'PRODUCTS:PRODUCTSLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IProductsFilterViewModel ) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type ProductsAction = ChangeFilter | DeleteFilter;
