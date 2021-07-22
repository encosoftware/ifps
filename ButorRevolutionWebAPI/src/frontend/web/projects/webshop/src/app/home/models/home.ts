import { IImageDetailsModel } from '../../shared/models/shared';

export interface IGroupingCategoryWebshopViewModel {
    id: number;
    name?: string;
    image?: IImageDetailsModel;
    subCategories?: IGroupingSubCategoryWebshopViewModel[];
}

export interface IGroupingSubCategoryWebshopViewModel {
    id: number;
    name?: string;
    image?: IImageDetailsModel;
}
