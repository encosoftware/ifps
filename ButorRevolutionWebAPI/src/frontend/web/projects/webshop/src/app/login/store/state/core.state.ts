import { LanguageTypeEnum } from '../../../shared/clients';
import { TokenModel, TokenLoginModel } from '../../../shared/models/auth';


export interface ICoreLanguageState {
    language: LanguageTypeEnum;
}

export interface ICoreLoginState {
    token: TokenModel;
}

export interface ILoginTState {
    loginToken: TokenLoginModel;
}


