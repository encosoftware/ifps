import { combineReducers } from '@ngrx/store';
import { LanguageTypeEnum } from '../../../shared/clients';
import { CoreAction, ActionTypes, CoreActionToken, CoreActionClaims, CoreActionLogin } from '../actions/core.actions';
import { TokenModel, TokenLoginModel } from '../../models/auth';

const initialLanguageState: LanguageTypeEnum = LanguageTypeEnum.EN;
const initialLoginState: TokenModel = ({
    accessToken: '',
    refreshToken: ''
});
const initialLoginTokenState: TokenLoginModel = ({
    accessToken: '',
    refreshToken: '',
    Email: '',
    ImageContainerName: '',
    ImageFileName: '',
    Language: '',
    RoleName: '',
    UserName: '',
    IFPSClaim: [],
    UserId: '',
    CompanyId: ''
});

function coreLoginTokenChoosenReducers(
    state: TokenLoginModel = initialLoginTokenState,
    action: CoreActionLogin
): TokenLoginModel {

    switch (action.type) {
        case ActionTypes.Login:
            return action.payload;
    }

    return state;
}

export const coreLoginTokenReducers = combineReducers({
    loginToken: coreLoginTokenChoosenReducers
});

function coreLanguageChoosenReducers(
    state: LanguageTypeEnum = initialLanguageState,
    action: CoreAction
): LanguageTypeEnum {

    switch (action.type) {
        case ActionTypes.Language:
            return action.payload;
    }

    return state;
}

export const coreLanguageReducers = combineReducers({
    language: coreLanguageChoosenReducers
});

function coreLoginChoosenReducers(
    state: TokenModel = initialLoginState,
    action: CoreActionToken
): TokenModel {

    switch (action.type) {
        case ActionTypes.Token:
            return { ...action.payload };
    }

    return { ...state };
}

export const coreLoginReducers = combineReducers({
    token: coreLoginChoosenReducers
});

function coreClaimsChoosenReducers(
    state: string[] = [],
    action: CoreActionClaims
): string[] {

    switch (action.type) {
        case ActionTypes.Claims:
            return action.payload ? action.payload : [];
    }

    return state ? state : [];
}

export const coreClaimsReducers = combineReducers({
    claims: coreClaimsChoosenReducers
});

