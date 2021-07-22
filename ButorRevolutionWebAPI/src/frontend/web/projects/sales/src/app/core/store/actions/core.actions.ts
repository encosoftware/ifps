import { Action } from '@ngrx/store';
import { LanguageTypeEnum } from '../../../shared/clients';
import { TokenModel, TokenLoginModel } from '../../models/auth';

export enum ActionTypes {
    Language = 'BUTOR:LANGUAGE',
    Token = 'BUTOR:TOKEN',
    Claims = 'BUTOR:CLAIMS',
    Login = 'BUTOR:LOGINTOKEN'
}

export class Language implements Action {
    readonly type = ActionTypes.Language;

    constructor(public payload: LanguageTypeEnum) { }
}
export class Token implements Action {
    readonly type = ActionTypes.Token;

    constructor(public payload: TokenModel) { }
}
export class Claims implements Action {
    readonly type = ActionTypes.Claims;

    constructor(public payload: string[]) { }
}

export class LoginToken implements Action {
    readonly type = ActionTypes.Login;

    constructor(public payload: TokenLoginModel) { }
}



export type CoreAction = Language;
export type CoreActionToken = Token;
export type CoreActionClaims = Claims;
export type CoreActionLogin = LoginToken;
