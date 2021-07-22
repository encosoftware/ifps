export interface IClaim {
    id: number;
    name: string;
    enabled: boolean;
}

export interface IModule {
    name: any;
    detail: string;
    checkmarkClass?: string;
    enabled?: boolean;
    claims: IClaim[];
}
