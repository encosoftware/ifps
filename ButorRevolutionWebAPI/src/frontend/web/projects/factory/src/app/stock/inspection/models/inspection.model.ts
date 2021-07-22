import { PagerModel } from '../../../shared/models/pager.model';

export interface IInspectionListViewModel {
    id: number;
    date: Date;
    stock: string;
    report: string;
    delegation: string[];
    isClosed: boolean;
}

export interface IInspectionFilterListViewModel {
    date?: Date;
    stock?: number;
    report?: string;
    delegation?: string;
    pager: PagerModel;
}

export interface INewInspectionViewModel {
    inspectedOn: Date;
    reportName: string;
    storageId: number;
    delegationIds: number[];
}

export interface IInspectionViewModel {
    isClosed: boolean;
    basicInfo: IInspectionHeaderViewModel;
    products: IInspectionProductListViewModel[];
}

export interface IInspectionHeaderViewModel {
    date: Date;
    stock: string;
    delegation: string[];
}

export interface IInspectionProductListViewModel {
    id: number;
    description: string;
    code: string;
    cellName: string;
    cellMeta: string;
    quantity: number;
    isCorrect?: boolean;
    missing?: number;
}


export interface IStocksDropDownViewModel {
    id: number;
    name: string;
}

export interface IUserDropDownViewModel {
    id: number;
    name: string;
}
