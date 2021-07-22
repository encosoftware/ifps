
export interface IDivisionViewModel {
    id: number;
    name: string;
    roles: IRoleViewModel[];
}

export interface IRoleViewModel {
    id?: number;
    name: string;
}

export interface INewRoleViewModel {
    divisionId: number;
    name: string;
}

export interface IDivisionListViewModel {
    id: number;
    name?: string;
}
