import { IPagerModel } from '../../../shared/models/pager.model';

export interface IWorkstationsListModel {
    id: number;
    name: string;
    optimalCrew: number;
    machine: string;
    type: string;
    statusText: string;
    status: boolean;
}

export interface IWorkstationsFilterModel {
    name?: string;
    optimalCrew?: number;
    machine?: number;
    type?: number;
    status?: boolean;

    pager: IPagerModel;
}

export interface IWorkstationDetailsModel {
    id?: number;
    isActive?: boolean;
    name: string;
    optimalCrew: number;
    machine: number;
    type?: number;
}

export interface IWorkstationCameraDetailsModel {
    start?: IWorkstationCameraListModel;
    finish?: IWorkstationCameraListModel;
}

export interface IWorkstationCameraListModel {
    camera?: number;
    state?: number;
}

export interface IWorkstationCameraCreateModel {
    start?: IWorkstationCameraListModel;
    finish?: IWorkstationCameraListModel;
}

export interface ICameraDropdownModel {
    id: number;
    name: string;
}

export interface IMachineDropdownModel {
    id: number;
    name: string;
}

export interface ICFCProductionStateDropdownModel {
    id: number;
    name: string;
}

export interface IWorkstationDropdownModel {
    id: number;
    name: string;
}
