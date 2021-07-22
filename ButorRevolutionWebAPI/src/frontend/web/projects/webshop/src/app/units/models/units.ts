import { IImageDetailsModel, PriceListModel } from '../../shared/models/shared';
import { IPagerModel } from '../../shared/models/pager.model';
import { FurnitureUnitTypeEnum } from '../../shared/clients';

export interface OrderingViewModel {
    column?: string;
    isDescending: boolean;
}

export interface IFurnitureUnitListByWebshopCategoryViewModel {
    furnitureUnitId: number;
    categoryId?: number | undefined;
    code?: string | undefined;
    description?: string | undefined;
    width: number;
    height: number;
    depth: number;
    image?: IImageDetailsModel | undefined;
    price?: PriceListModel | undefined;
}

export interface IFurnitureUnitListByWebshopCategoryFilterViewModel extends IPagerModel {
    subcategoryId?: number;
    unitType?: FurnitureUnitTypeEnum;
    minimumPrice?: number;
    maximumPrice?: number;
    Orderings?: OrderingViewModel[];
}
