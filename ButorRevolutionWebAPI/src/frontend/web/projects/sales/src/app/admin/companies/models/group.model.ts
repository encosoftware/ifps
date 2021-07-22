import { IPictureModel } from '../../materials/models/worktops.model';

export interface IUserTeamModel {
    id?: number;
    name?: string;
    typeName?: string;
    userTeamType?: number;
    users?: IUserListModel[];
}

export interface IUserListModel {
    id: number;
    name?: string | undefined;
    phoneNumber?: string | undefined;
    email?: string | undefined;
    picture?: IPictureModel;
}
