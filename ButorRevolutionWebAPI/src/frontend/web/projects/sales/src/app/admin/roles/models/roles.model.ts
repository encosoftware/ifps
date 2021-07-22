
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
    categoryId: number;
    name: string;
}

export interface ICategoryShortViewModel {
    id: number;
    name?: string;
}
