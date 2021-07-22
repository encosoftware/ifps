import { LanguageTypeEnum } from '../../shared/clients';

export interface TokenModel {
    accessToken?: string;
    refreshToken?: string;
}

export interface TokenLoginModel {
    accessToken: string;
    refreshToken: string;
    Email: string;
    ImageContainerName: string;
    ImageFileName: string;
    Language: string;
    RoleName: string;
    UserName: string;
    IFPSClaim: string[];
    UserId: string;
    CompanyId: string;
}

export interface LoginViewModel {
    email: string;
    password: string;
    rememberMe: boolean;
}

export interface IPasswordChangeViewModel {
    password: string;
}

export interface IMenuViewModel {
    id: number;
    name?: string;
    companyId?: number;
    language: LanguageTypeEnum;
    roleName?: string;
    menuClaims?: IMenuClaim[];
    image?: ImageDetailsViewModel;
}

export interface ImageDetailsViewModel {
    containerName?: string;
    fileName?: string;
}

export interface IMenuClaim {
    name?: string;
    claimType: string;
}

export interface JwtTokenModel {
    Email: string;
    ImageContainerName: string;
    ImageFileName: string;
    Language: string;
    RoleName: string;
    UserName: string;
    IFPSClaim: string[];
    UserId: string;
    CompanyId: string;
}

export interface coreAllModel {
    accessToken: string;
    refreshToken: string;
    Email: string;
    ImageContainerName: string;
    ImageFileName: string;
    Language: string;
    RoleName: string;
    UserName: string;
    IFPSClaim: string[];
    UserId: string;
    language: LanguageTypeEnum;
}