import { Action } from '@ngrx/store';
import { LanguageTypeEnum } from '../../../shared/clients';
import { TokenModel, TokenLoginModel } from '../../../shared/models/auth';

export enum ActionTypes {
    Language = 'WEBSHOP:LANGUAGE',
    Token = 'WEBSHOP:TOKEN',
    Login = 'WEBSHOP:LOGINTOKEN'
}

export class Language implements Action {
    readonly type = ActionTypes.Language;

    constructor(public payload: LanguageTypeEnum) { }
}
export class Token implements Action {
    readonly type = ActionTypes.Token;

    constructor(public payload: TokenModel) { }
}


export class LoginToken implements Action {
    readonly type = ActionTypes.Login;

    constructor(public payload: TokenLoginModel) { }
}



export type CoreAction = Language;
export type CoreActionToken = Token;
export type CoreActionLogin = LoginToken;
