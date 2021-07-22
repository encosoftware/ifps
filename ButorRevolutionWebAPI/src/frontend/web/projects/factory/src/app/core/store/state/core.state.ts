import { LanguageTypeEnum } from '../../../shared/clients';
import { TokenModel, TokenLoginModel } from '../../models/auth';

export interface ICoreLanguageState {
    language: LanguageTypeEnum;
}

export interface ICoreLoginState {
    token: TokenModel;
}

export interface ICoreClaimsState {
    claims: string[];
}

export interface ILoginTState {
    loginToken: TokenLoginModel;
}


