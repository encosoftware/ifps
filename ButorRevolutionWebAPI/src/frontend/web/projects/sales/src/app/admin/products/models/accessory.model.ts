import { IPictureModel } from './products.models';

export interface IProductsAccessoriesViewModel {
    furnitureUnitId?: string;
    id?: number;
    name: string;
    amount: number;
    materialCode?: string;
    materialId?: string;
    picture?: IPictureModel;
    src?: string;
    // size: ISizeModel;
    // edging?: IEdgingModel;
}
