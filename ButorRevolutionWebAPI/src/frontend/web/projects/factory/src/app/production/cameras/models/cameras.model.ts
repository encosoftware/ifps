import { IPagerModel } from '../../../shared/models/pager.model';

export interface ICameraListModel {
    id: number;
    name: string;
    type: string;
    ipAddress: string;
}

export interface ICameraFilterModel {
    name?: string;
    type?: string;
    ipAddress?: string;
    pager: IPagerModel;
}

export interface ICameraDetailsModel {
    id?: number;
    name: string;
    type: string;
    ipAddress: string;
}
