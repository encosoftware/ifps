import { GroupingCategoryEnum, LanguageTypeEnum } from '../../../shared/clients';

export interface Category {
    id: number;
    name: string | any;
    type: GroupingCategoryEnum;
    children?: Category[];
}
export interface IViewCategory {
    id: number;
    name?: string | undefined;
    groupingCategoryType: GroupingCategoryEnum;
    parentId?: number | undefined;
    children?: IViewCategory[] | undefined;
}

export interface EditCategory {
    id?: number;
    translation: ITranslatable[];
    parentId: number;
    type?: GroupingCategoryEnum;
}
export interface CreateCategory {
    translation: ITranslatable[];
    parentId: number;
}

export interface ExampleFlatNode {
    expandable: boolean;
    name: string;
    level: number;
}
export interface FilterCategories {
    id?: number;
    name?: string;
}

export interface ITranslatable {
    id: number;
    name?: string;
    language: LanguageTypeEnum;
}
