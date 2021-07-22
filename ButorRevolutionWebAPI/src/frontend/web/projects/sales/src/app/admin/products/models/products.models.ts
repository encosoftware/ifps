import { PagerModel } from '../../../shared/models/pager.model';
import { IProductsFrontViewModel } from './front.model';
import { IProductsCorpusViewModel } from './corpus.model';
import { IProductsAccessoriesViewModel } from './accessory.model';

export interface IProductsListViewModel {
    id: string;
    src: string;
    description: string;
    category: string;
    categoryId: number;
    code: string;
    size: ISizeModel;
    materialCost?: number;
    materialCurrency?: string;
    sellPrice?: number;
    sellPriceCurrencyId?: number;
}

export interface IProductCreateModel {
    code: string;
    description: string;
    size: ISizeModel;
    categoryId: number;
    picture: IPictureModel;
    furnitureUnitTypeId: number;
}

export interface IProductDetailsModel {
    code: string;
    description: string;
    category?: string;
    categoryId: number;
    size: ISizeModel;
    picture: IPictureModel;
    front?: IProductsFrontViewModel[];
    corpus?: IProductsCorpusViewModel[];
    accessories?: IProductsAccessoriesViewModel[];
}

export interface IProductsFilterViewModel {
    description?: string;
    code?: string;
    category?: string;
    categoryId?: number;
    pager: PagerModel;
}

export interface ISizeModel {
    width?: number;
    depth?: number;
    height?: number;
}

export interface IEdgingModel {
    all?: string;
    isChecked?: boolean;
    top?: string;
    bottom?: string;
    right?: string;
    left?: string;
}

export interface IPictureModel {
    containerName?: string;
    fileName?: string;
}

export interface IFoilListModel {
    id: string;
    code: string;
}

export interface IFurnitureUnitCategoryModel {
    id: number;
    name: string;
}
