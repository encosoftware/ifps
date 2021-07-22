import { IPagerModel } from '../../../shared/models/pager.model';

export interface IFurnitureUnitModel {
    id: string;
    code: string;
    description: string;
    category: string;
    isUploaded: boolean;
}

export interface IFurnitureUnitFilterModel {
    code?: string;
    description?: string;
    isUploaded?: boolean;
    category?: string;
    pager: IPagerModel;
}

export interface IFurnitureUnitDocumentUploadModel {
    furnitureUnitId: string;
    containerName: string;
    fileName: string;
}

export interface IUploadingFile {
    name: string;
    currentSize: number;
    totalSize: number;
    percentage: number;
}

export interface IOrderUploadedDocumentItem {
    fileName: string;
    containerName: string;
    originalFileName: string;
}
