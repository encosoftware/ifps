import { ISizeModel, IEdgingModel, IPictureModel } from './products.models';

export interface IProductsFrontViewModel {
    id: string;
    src?: string;
    name: string;
    code: string;
    size: ISizeModel;
    amount: number;
    edging?: IEdgingModel;
    picture?: IPictureModel;
}

export interface IProductsFrontDetailsViewModel {
    furnitureUnitId?: string;
    id?: number;
    name: string;
    materialId: string;
    picture?: IPictureModel;
    src?: string;
    size: ISizeModel;
    amount: number;
    allId?: number;
    topId: string;
    bottomId: string;
    rightId: string;
    leftId: string;
    typeId?: number;
}

export interface IMaterialCodeViewModel {
    id: string;
    code: string;
    fileName?: string;
    containerName?: string;
    src?: string;
    description?: string;
}
