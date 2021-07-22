import { IPictureModel } from "../../materials/models/decorboards.model";

export interface IUserTeamModel {
    id?: number;
    name?: string;
    users?: IUserListModel[];
}

export interface IUserListModel {
    id: number;
    name?: string | undefined;
    phoneNumber?: string | undefined;
    email?: string | undefined;
    picture?: IPictureModel;
}
