import { ISizeModel, IEdgingModel, IPictureModel } from './products.models';

export interface IProductsCorpusViewModel {
    id: string;
    src?: string;
    name: string;
    code: string;
    size: ISizeModel;
    amount: number;
    edging?: IEdgingModel;
    picture?: IPictureModel;
}

export interface IProductsCorpusDetailsViewModel {
    furnitureUnitId?: string;
    id?: number;
    name: string;
    materialId: string;
    picture?: IPictureModel;
    size: ISizeModel;
    amount: number;
    topId: string;
    bottomId: string;
    rightId: string;
    leftId: string;
    allId?: number;
    typeId: number;
}
